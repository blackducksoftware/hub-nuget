using System;
using Microsoft.Build.Framework;
using System.ComponentModel;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Threading;
using NuGet.Protocol;
using NuGet.Protocol.Core.v2;
using NuGet.Configuration;
using System.Diagnostics;
using System.IO;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using Com.Blackducksoftware.Integration.Hub.Nuget.Model;
using Newtonsoft.Json.Linq;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    public class BuildBOMTask : Microsoft.Build.Utilities.Task
    {
        #region Hub Properties
        [Required]
        public string HubUrl { get; set; }

        [Required]
        public string HubUsername { get; set; }

        [Required]
        public string HubPassword { get; set; }

        public int HubTimeout { get; set; } = 120;
        public string HubProjectName { get; set; }
        public string HubVersionName { get; set; }
        #endregion

        #region Proxy Properties
        public string HubProxyHost { get; set; }
        public string HubProxyPort { get; set; }
        public string HubProxyUsername { get; set; }
        public string HubProxyPassword { get; set; }

        #endregion

        public int HubScanTimeout { get; set; } = 300;
        public string OutputDirectory { get; set; }
        public string ExcludedModules { get; set; } = "";
        public bool HubIgnoreFailure { get; set; } = false;
        public bool CreateFlatDependencyList { get; set; } = false;
        public bool CreateHubBdio { get; set; } = false;
        public bool DeployHubBdio { get; set; } = false;
        public bool CreateHubReport { get; set; } = false;
        public bool CheckPolicies { get; set; } = false;

        public string PackagesConfigPath { get; set; }
        public string PackagesRepoPath { get; set; }

        public override bool Execute()
        {
            // Creates output directory if it doesn't already exist
            Directory.CreateDirectory(OutputDirectory);

            if (CreateFlatDependencyList)
            {
                List<NuGet.PackageReference> packages = GenerateFlatList();
                BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
                using (StreamWriter file = new StreamWriter($"{OutputDirectory}/{HubProjectName}_flat.txt", false, Encoding.UTF8))
                {
                    foreach (NuGet.PackageReference packageReference in packages)
                    {
                        string externalId = bdioPropertyHelper.CreateNugetExternalId(packageReference.Id, packageReference.Version.ToString());
                        file.WriteLine(externalId);
                    }
                }
            }

            if (CreateHubBdio)
            {
                BdioContent bdioContent = BuildBOM();
                File.WriteAllText($"{OutputDirectory}/{HubProjectName}.jsonld", bdioContent.ToString());
            }

            if (DeployHubBdio)
            {
                //Task deployTask = Deploy(bdioContent);
                //deployTask.Wait();
            }

            if (CreateHubReport)
            {

            }

            if (CheckPolicies)
            {

            }

            return true;
        }

        #region Make Flat List
        public List<NuGet.PackageReference> GenerateFlatList()
        {
            // Load the packages.config file into a list of Packages
            NuGet.PackageReferenceFile configFile = new NuGet.PackageReferenceFile(PackagesConfigPath);
            return new List<NuGet.PackageReference>(configFile.GetPackageReferences());
        }
        #endregion

        #region Generate BDIO
        public BdioContent BuildBOM()
        {
            // Load the packages.config file into a list of Packages
            NuGet.PackageReferenceFile configFile = new NuGet.PackageReferenceFile(PackagesConfigPath);

            // Setup NuGet API
            // Snippets taken from https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-2 with modifications
            List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3());  // Add v3 API support
            providers.AddRange(Repository.Provider.GetCoreV2());  // Add v2 API support
            PackageSource packageSource = new PackageSource(PackagesRepoPath);
            SourceRepository sourceRepository = new SourceRepository(packageSource, providers);
            PackageMetadataResource packageMetadataResource = sourceRepository.GetResource<PackageMetadataResource>();

            // Create BDIO 
            BdioContent bdioContent = BuildBOMFromMetadata(new List<NuGet.PackageReference>(configFile.GetPackageReferences()), packageMetadataResource);
            return bdioContent;
        }

        public BdioContent BuildBOMFromMetadata(List<NuGet.PackageReference> packages, PackageMetadataResource metadataResource)
        {
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioNodeFactory bdioNodeFactory = new BdioNodeFactory(bdioPropertyHelper);
            BdioContent bdio = new BdioContent();

            // Create bdio bill of materials node
            BdioBillOfMaterials bdioBillOfMaterials = bdioNodeFactory.CreateBillOfMaterials(HubProjectName);

            // Create bdio project node
            string projectBdioId = bdioPropertyHelper.CreateBdioId(HubProjectName, HubVersionName);
            BdioExternalIdentifier projectExternalIdentifier = bdioPropertyHelper.CreateNugetExternalIdentifier(HubProjectName, HubVersionName); // Note: Could be different. Look at config file
            BdioProject bdioProject = bdioNodeFactory.CreateProject(HubProjectName, HubVersionName, projectBdioId, projectExternalIdentifier);

            // Create relationships for every bdio node
            List<BdioNode> bdioComponents = new List<BdioNode>();
            foreach (NuGet.PackageReference packageRef in packages)
            {
                // Create component node
                string componentName = packageRef.Id;
                string componentVersion = packageRef.Version.ToString();
                string componentBdioId = bdioPropertyHelper.CreateBdioId(componentName, componentVersion);
                BdioExternalIdentifier componentExternalIdentifier = bdioPropertyHelper.CreateNugetExternalIdentifier(componentName, componentVersion);
                BdioComponent component = bdioNodeFactory.CreateComponent(componentName, componentVersion, componentBdioId, componentExternalIdentifier);

                // Add references
                List<PackageDependency> packageDependencies = GetPackageDependencies(packageRef, metadataResource);
                foreach (PackageDependency packageDependency in packageDependencies)
                {
                    // Create node from dependency info
                    string dependencyName = packageDependency.Id;
                    string dependencyVersion = GetDependencyVersion(packageDependency, packages);
                    string dependencyBdioId = bdioPropertyHelper.CreateBdioId(dependencyName, dependencyVersion);
                    BdioExternalIdentifier dependencyExternalIdentifier = bdioPropertyHelper.CreateNugetExternalIdentifier(dependencyName, dependencyVersion);
                    BdioComponent dependency = bdioNodeFactory.CreateComponent(dependencyName, dependencyVersion, dependencyBdioId, dependencyExternalIdentifier);

                    // Add relationship
                    bdioPropertyHelper.AddRelationship(component, dependency);
                }

                bdioComponents.Add(component);
            }

            bdio.BillOfMaterials = bdioBillOfMaterials;
            bdio.Project = bdioProject;
            bdio.Components = bdioComponents;

            return bdio;
        }

        public void WriteBdio(BdioContent bdio, TextWriter textWriter)
        {
            BdioWriter writer = new BdioWriter(textWriter);
            writer.WriteBdioNode(bdio.BillOfMaterials);
            writer.WriteBdioNode(bdio.Project);
            writer.WriteBdioNodes(bdio.Components);
            writer.Dispose();
        }

        private string GetDependencyVersion(PackageDependency packageDependency, List<NuGet.PackageReference> packages)
        {
            string version = null;
            foreach (NuGet.PackageReference packageRef in packages)
            {
                if (packageRef.Id == packageDependency.Id)
                {
                    version = packageRef.Version.ToString();
                    break;
                }
            }
            return version;
        }

        public List<PackageDependency> GetPackageDependencies(NuGet.PackageReference packageDependency, PackageMetadataResource metadataResource)
        {
            List<PackageDependency> dependencies = new List<PackageDependency>();

            //Gets all versions of package in package repository
            List<IPackageSearchMetadata> matchingPackages = new List<IPackageSearchMetadata>(metadataResource.GetMetadataAsync(packageDependency.Id, true, true, new Logger(), CancellationToken.None).Result);
            foreach (IPackageSearchMetadata matchingPackage in matchingPackages)
            {
                // Check if the matching package is the same as the version defined
                if (matchingPackage.Identity.Version.ToString() == packageDependency.Version.ToString())
                {
                    // Gets every dependency set in the package
                    foreach (PackageDependencyGroup dependencySet in matchingPackage.DependencySets)
                    {
                        // Grab the dependency set for the target framework. We only care about majors and minors in the version
                        if (FrameworksMatch(dependencySet, packageDependency))
                        {
                            dependencies.AddRange(dependencySet.Packages);
                            break;
                        }
                    }
                    break;
                }
            }
            return dependencies;
        }

        private bool FrameworksMatch(PackageDependencyGroup framework1, NuGet.PackageReference framework2)
        {
            bool majorMatch = framework1.TargetFramework.Version.Major == framework2.TargetFramework.Version.Major;
            bool minorMatch = framework1.TargetFramework.Version.Minor == framework2.TargetFramework.Version.Minor;
            return majorMatch && minorMatch;
        }
        #endregion

        #region Deploy
        public async Task Deploy(BdioContent bdioContent)
        {
            using (HttpClient client = await CreateClient())
            {
                await LinkedDataAPI(client, bdioContent);
                await WaitForScanComplete(client);
            }
        }

        public async Task WaitForScanComplete(HttpClient client)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            string codeLocationId = null;

            while (stopwatch.ElapsedMilliseconds / 1000 < HubScanTimeout)
            {
                Console.WriteLine("Checking scan summary status");
                PageCodeLocationView codeLocations = await CodeLocationsAPI(client);
                if (codeLocations.TotalCount > 0)
                {
                    CodeLocationView codeLocation = codeLocations.Items[0];
                    codeLocationId = codeLocation.Metadata.GetFirstId(codeLocation.Metadata.Href);
                    break;
                }
                else
                {
                    Console.WriteLine("No code locations found. Trying again...");
                }
            }

            if (codeLocationId == null)
            {
                throw new BlackDuckIntegrationException($"Failed to get the codelocation for {HubProjectName} ");
            }

            string currentStatus = "UNKOWN";
            while (stopwatch.ElapsedMilliseconds / 1000 < HubScanTimeout)
            {
                PageScanSummaryView scanSummaries = await ScanSummariesAPI(client, codeLocationId);
                if (scanSummaries.TotalCount == 0)
                {
                    throw new BlackDuckIntegrationException($"There are no scan summaries for id: {codeLocationId}");
                }
                ScanSummaryView scanSummary = scanSummaries.Items[0];
                string scanStatus = scanSummary.Status;

                if (!scanStatus.Equals(currentStatus))
                {
                    currentStatus = scanStatus;
                    Console.WriteLine($"\tScan Status = {currentStatus} @ {stopwatch.ElapsedMilliseconds / 1000.0}");
                }

                if (currentStatus.Equals("COMPLETE"))
                {
                    stopwatch.Stop();
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                }
            }

            if (stopwatch.ElapsedMilliseconds / 1000 > HubScanTimeout)
            {
                throw new BlackDuckIntegrationException($"Scanning of the codelocation: {codeLocationId} execeded the {HubScanTimeout} second timeout");
            }
        }

        public void GenerateReports(HttpClient client)
        {

        }

        public void CheckPolicy(HttpClient client)
        {

        }

        public async Task<PageScanSummaryView> ScanSummariesAPI(HttpClient client, string codeLocationId)
        {
            HttpResponseMessage response = await client.GetAsync($"{HubUrl}/api/codelocations/{codeLocationId}/scan-summaries?sort=updated%20asc");
            VerifySuccess(response);
            string content = await response.Content.ReadAsStringAsync();
            PageScanSummaryView scanSummaries = JToken.Parse(content).FromJToken<PageScanSummaryView>();
            return scanSummaries;
        }

        public async Task<PageCodeLocationView> CodeLocationsAPI(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync($"{HubUrl}/api/codelocations?q={HubProjectName}%2F0.0&codeLocationType=BOM_IMPORT");
            VerifySuccess(response);
            string content = await response.Content.ReadAsStringAsync();
            PageCodeLocationView codeLocations = JToken.Parse(content).FromJToken<PageCodeLocationView>();
            return codeLocations;
        }

        public async Task LinkedDataAPI(HttpClient client, BdioContent bdio)
        {
            HttpContent content = new StringContent(bdio.ToString(), Encoding.UTF8, "application/ld+json");
            HttpResponseMessage response = await client.PostAsync($"{HubUrl}/api/bom-import", content);
            VerifySuccess(response);
        }

        public async Task<HttpClient> CreateClient()
        {
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, HubTimeout);

            Dictionary<string, string> credentials = new Dictionary<string, string>
            {
                {"j_username", HubUsername },
                {"j_password", HubPassword }
            };

            HttpContent content = new FormUrlEncodedContent(credentials);
            HttpResponseMessage response = await client.PostAsync($"{HubUrl}/j_spring_security_check", content);
            VerifySuccess(response);

            return client;
        }

        private void VerifySuccess(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new BlackDuckIntegrationException(response);
            }
        }



    }
    #endregion

    public class Logger : NuGet.Common.ILogger
    {
        public void LogDebug(string data) => Trace.WriteLine($"DEBUG: {data}");
        public void LogVerbose(string data) => Trace.WriteLine($"VERBOSE: {data}");
        public void LogInformation(string data) => Trace.WriteLine($"INFORMATION: {data}");
        public void LogMinimal(string data) => Trace.WriteLine($"MINIMAL: {data}");
        public void LogWarning(string data) => Trace.WriteLine($"WARNING: {data}");
        public void LogError(string data) => Trace.WriteLine($"ERROR: {data}");
        public void LogSummary(string data) => Trace.WriteLine($"SUMMARY: {data}");

        public void LogInformationSummary(string data)
        {
            throw new NotImplementedException();
        }

        public void LogErrorSummary(string data)
        {
            throw new NotImplementedException();
        }
    }
}