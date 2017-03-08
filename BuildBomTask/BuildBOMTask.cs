using System;
using Microsoft.Build.Framework;
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
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    public class BuildBOMTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string HubUrl { get; set; }
        [Required]
        public string HubUsername { get; set; }
        [Required]
        public string HubPassword { get; set; }
        [Required]
        public string PackagesRepoUrl { get; set; }

        public int HubTimeout { get; set; } = 120;
        public string HubProjectName { get; set; }
        public string HubVersionName { get; set; }

        public string HubProxyHost { get; set; }
        public string HubProxyPort { get; set; }
        public string HubProxyUsername { get; set; }
        public string HubProxyPassword { get; set; }

        public int HubScanTimeout { get; set; } = 300;
        public string OutputDirectory { get; set; }
        public string ExcludedModules { get; set; } = "";
        public bool HubIgnoreFailure { get; set; } = false;
        public bool CreateFlatDependencyList { get; set; } = false;
        public bool CreateHubBdio { get; set; } = true;
        public bool DeployHubBdio { get; set; } = true;
        public bool CreateHubReport { get; set; } = false;
        public bool CheckPolicies { get; set; } = false;

        public string PackagesConfigPath { get; set; }

        // Currently public for testing
        public CodeLocationDataService CodeLocationDataService;
        public ScanSummariesDataService ScanSummariesDataService;
        public DeployBdioDataService DeployBdioDataService;
        public ProjectDataService ProjectDataService;
        public PolicyDataService PolicyDataService;
        public RiskReportDataService RiskReportDataService;

        public RestConnection RestConnection;
        public string BdioId;

        public override bool Execute()
        {
            if (HubIgnoreFailure)
            {
                try
                {
                    ExecuteTask();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{exception}");
                }
            }
            else
            {
                ExecuteTask();
            }

            return true;
        }

        private void ExecuteTask()
        {
            // Reset Connection Setup
            if (RestConnection == null)
            {
                HubCredentials credentials = new HubCredentials(HubUsername, HubPassword);
                HubCredentials proxyCredentials = new HubCredentials(HubProxyUsername, HubProxyPassword);
                HubProxyInfo proxyInfo = new HubProxyInfo(HubProxyHost, HubProxyPort, proxyCredentials);
                HubServerConfig hubServerConfig = new HubServerConfig(HubUrl, HubTimeout, credentials, proxyInfo);
                RestConnection restConnection = new CredentialsResetConnection(hubServerConfig);
                RestConnection = restConnection;
            }

            // Setup DataServices
            if (CodeLocationDataService == null)
            {
                CodeLocationDataService = new CodeLocationDataService(RestConnection);
            }
            if (ScanSummariesDataService == null)
            {
                ScanSummariesDataService = new ScanSummariesDataService(RestConnection);
            }
            if (DeployBdioDataService == null)
            {
                DeployBdioDataService = new DeployBdioDataService(RestConnection);
            }
            if (ProjectDataService == null)
            {
                ProjectDataService = new ProjectDataService(RestConnection);
            }
            if (PolicyDataService == null)
            {
                PolicyDataService = new PolicyDataService(RestConnection);
            }
            if (RiskReportDataService == null)
            {
                RiskReportDataService = new RiskReportDataService(RestConnection);
            }

            // Set helper properties
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioId = bdioPropertyHelper.CreateBdioId(HubProjectName, HubVersionName);

            // Creates output directory if it doesn't already exist
            Directory.CreateDirectory(OutputDirectory);
            string bdioFilePath = $"{OutputDirectory}/{HubProjectName}.jsonld";

            if (CreateFlatDependencyList)
            {
                List<NuGet.PackageReference> packages = GenerateFlatList();
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
                Console.WriteLine($"Buidling BDIO");
                BdioContent bdioContent = BuildBOM();
                File.WriteAllText(bdioFilePath, bdioContent.ToString());
                Console.WriteLine($"Created Bdio @ {bdioFilePath}");
            }

            if (DeployHubBdio)
            {
                Console.WriteLine($"Deploying BDIO to hub @ {HubUrl}");

                string bdio = File.ReadAllText(bdioFilePath);
                BdioContent bdioContent = BdioContent.Parse(bdio);

                CodeLocationView codeLocation = CodeLocationDataService.GetCodeLocationView(bdioContent.Project.Id);
                int currentSummaries = ScanSummariesDataService.GetScanSummaries(codeLocation).TotalCount;

                HttpResponseMessage response = DeployBdioDataService.Deploy(bdioContent);

                WaitForScanComplete(RestConnection, currentSummaries);
                Console.WriteLine("Finished deployment");
            }

            if (CheckPolicies)
            {
                Console.WriteLine($"Checking policies of {HubProjectName}");

                PolicyStatus policyStatus = new PolicyStatus(GetPolicies());

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("The Hub found: ");
                stringBuilder.Append(policyStatus.InViolationCount);
                stringBuilder.Append(" components in violation, ");
                stringBuilder.Append(policyStatus.InViolationCount);
                stringBuilder.Append(" components in violation, but overridden, and ");
                stringBuilder.Append(policyStatus.NotInViolationCount);
                stringBuilder.Append(" components not in violation.");

                if (policyStatus.OverallStatus == PolicyStatusEnum.IN_VIOLATION)
                {
                    string error = $"The Hub found: {policyStatus.InViolationCount} components in violation\n";
                    throw new BlackDuckIntegrationException(error);
                }
            }

            if (CreateHubReport)
            {
                Project project = ProjectDataService.GetMostRecentProjectItem(HubProjectName);
                ReportData reportData = RiskReportDataService.GetReportData(project);
                File.WriteAllText($"{OutputDirectory}/ReportData.json", reportData.ToJson(Newtonsoft.Json.Formatting.Indented));
            }
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
            // we may need more code here around handling package sources.
            PackageSource packageSource = new PackageSource(PackagesRepoUrl);
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

        public void WaitForScanComplete(HttpClient client, int currentSummaries)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            CodeLocationView codeLocation = null;

            while (stopwatch.ElapsedMilliseconds / 1000 < HubScanTimeout)
            {
                Console.WriteLine("Checking scan summary status");
                codeLocation = CodeLocationDataService.GetCodeLocationView(BdioId);
                if (codeLocation != null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("No code locations found. Trying again...");
                    Thread.Sleep(500);
                }
            }

            if (codeLocation == null)
            {
                throw new BlackDuckIntegrationException($"Failed to get the codelocation for {HubProjectName} ");
            }

            string codeLocationId = CodeLocationDataService.GetCodeLocationId(codeLocation);
            ScanStatusEnum currentStatus = ScanStatusEnum.UNSTARTED;
            while (stopwatch.ElapsedMilliseconds / 1000 < HubScanTimeout)
            {
                HubPagedResponse<ScanSummaryView> scanSummaries = ScanSummariesDataService.GetScanSummaries(codeLocation);
                if (scanSummaries == null || scanSummaries.Items == null)
                {
                    throw new BlackDuckIntegrationException($"There are no scan summaries for id: {codeLocationId}");
                }
                else if (scanSummaries.TotalCount > currentSummaries)
                {
                    ScanSummaryView scanSummary = scanSummaries.Items[0];
                    ScanStatusEnum scanStatus = scanSummary.Status;
                    if (!scanStatus.Equals(currentStatus))
                    {
                        currentStatus = scanStatus;
                        Console.WriteLine($"\tScan Status = {currentStatus} @ {stopwatch.ElapsedMilliseconds / 1000.0}");
                    }
                    if (currentStatus.Equals(ScanStatusEnum.COMPLETE))
                    {
                        stopwatch.Stop();
                        break;
                    }
                }
                Thread.Sleep(500);
            }

            if (stopwatch.ElapsedMilliseconds / 1000 > HubScanTimeout)
            {
                throw new BlackDuckIntegrationException($"Scanning of the codelocation: {codeLocationId} execeded the {HubScanTimeout} second timeout");
            }
        }

        #endregion        

        public VersionBomPolicyStatusView GetPolicies()
        {
            Project project = ProjectDataService.GetMostRecentProjectItem(HubProjectName);
            VersionBomPolicyStatusView policyStatus = PolicyDataService.GetVersionBomPolicyStatusView(project.ProjectId, project.VersionId);
            return policyStatus;
        }
    }

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