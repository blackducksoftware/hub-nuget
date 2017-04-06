using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService;
using Com.Blackducksoftware.Integration.Hub.Common.Net.DataService;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Core.v2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.BuildBom
{
    public class ProjectGenerator
    {
        public bool Verbose { get; set; } = false;
        public string HubUrl { get; set; }
        public string HubUsername { get; set; }
        public string HubPassword { get; set; }
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
        public string HubCodeLocationName { get; set; } = "";

        public string PackagesConfigPath { get; set; }

        public bool WaitForDeployment = false;
        private RestConnection RestConnection;

        // Dataservices
        private CodeLocationResponseService CodeLocationDataService;
        private ScanSummariesResponseService ScanSummariesDataService;
        private DeployBdioResponseService DeployBdioDataService;
        private ProjectResponseService ProjectDataService;
        private PolicyResponseService PolicyDataService;
        private RiskReportResponseService RiskReportDataService;
        private ScanStatusDataService ScanStatusDataService;

        // Helper properties
        private string BdioId;

        public virtual bool Execute()
        {
            try
            {
                Validate();
                Setup();
                ExecuteTask();
            }
            catch (Exception ex)
            {
                if (HubIgnoreFailure)
                {
                    Console.WriteLine("Error executing Build BOM task on project {0}, cause: {1}", HubProjectName, ex);
                    return true;
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                if (RestConnection != null)
                {
                    RestConnection.Dispose();
                }
            }
            return true;
        }

        private void Validate()
        {
            string message = "";

            StringBuilder messageBuilder = new StringBuilder(100);
            string huburl = HubUrl.Trim();
            string packagesRepo = PackagesRepoUrl.Trim();

            if(String.IsNullOrEmpty(packagesRepo))
            {
                messageBuilder.AppendLine("PackagesRepoUrl is required");
            }

            if(String.IsNullOrEmpty(huburl))
            {
                List<string> operationList = new List<string>(3);
                if(DeployHubBdio || CreateHubReport || CheckPolicies)
                {
                    messageBuilder.Append("Cannot perform the following operations because hub_url is empty: ");
                }

                if(DeployHubBdio)
                {
                    operationList.Add("deploy BDIO");
                }

                if (CreateHubReport)
                {
                    operationList.Add("create risk report");
                }

                if (CheckPolicies)
                {
                    operationList.Add("check policies");
                }

                if(operationList.Count > 0)
                {
                    messageBuilder.AppendLine(String.Join(", ", operationList));
                }
            }

            message = messageBuilder.ToString();

            if(!String.IsNullOrEmpty(message))
            {
                throw new BlackDuckIntegrationException(message);
            }
        }

        public void Setup()
        {
            if(String.IsNullOrWhiteSpace(PackagesConfigPath))
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                PackagesConfigPath = String.Format("{0}{1}packages.config",currentDirectory, Path.DirectorySeparatorChar);
            }

            if (!String.IsNullOrWhiteSpace(HubUrl.Trim()))
            {
                // Estabilish authenticated connection
                HubServerConfig hubServerConfig = BuildHubServerConfig();
                RestConnection restConnection = new CredentialsResetConnection(hubServerConfig);
                Setup(restConnection);
            }

            // Set helper properties
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioId = bdioPropertyHelper.CreateBdioId(HubProjectName, HubVersionName);
        }

        public void Setup(RestConnection restConnection)
        {
            RestConnection = restConnection;

            // Create required dataservices
            CodeLocationDataService = new CodeLocationResponseService(RestConnection);
            ScanSummariesDataService = new ScanSummariesResponseService(RestConnection);
            DeployBdioDataService = new DeployBdioResponseService(RestConnection);
            ProjectDataService = new ProjectResponseService(RestConnection);
            PolicyDataService = new PolicyResponseService(RestConnection);
            RiskReportDataService = new RiskReportResponseService(RestConnection);
            ScanStatusDataService = new ScanStatusDataService(RestConnection, Convert.ToInt64(TimeSpan.FromSeconds(Convert.ToDouble(HubScanTimeout)).TotalMilliseconds));
        }

        public void ExecuteTask()
        {
            if (IsExcluded())
            {
                Console.WriteLine("Project {0} excluded from task", HubProjectName);
            }
            else
            {
                // Creates output directory if it doesn't already exist
                Directory.CreateDirectory(OutputDirectory);

                // Define output files
                string bdioFilePath = $"{OutputDirectory}{Path.DirectorySeparatorChar}{HubProjectName}.jsonld";
                string flatListFilePath = $"{OutputDirectory}{Path.DirectorySeparatorChar}{HubProjectName}_flat.txt";

                // Execute task functionality
                if (CreateFlatDependencyList)
                {
                    string[] externalIds = CreateFlatList().ToArray();
                    File.WriteAllLines(flatListFilePath, externalIds, Encoding.UTF8);
                }

                if (CreateHubBdio)
                {
                    BdioContent bdioContent = BuildBOM();
                    File.WriteAllText(bdioFilePath, bdioContent.ToString());
                }

                if (DeployHubBdio)
                {
                    string bdio = File.ReadAllText(bdioFilePath);
                    BdioContent bdioContent = BdioContent.Parse(bdio);
                    DeployBdioDataService.Deploy(bdioContent);
                }

                // Only wait for scan if we have to
                if (DeployHubBdio && (CheckPolicies || CreateHubBdio || WaitForDeployment))
                {
                    WaitForHub();
                }

                if (CreateHubReport)
                {
                    ProjectView projectView = ProjectDataService.GetProjectView(HubProjectName);
                    ProjectVersionView projectVersionView = ProjectDataService.GetMostRecentVersion(projectView);
                    ReportData reportData = RiskReportDataService.GetReportData(projectView, projectVersionView);
                    RiskReportDataService.WriteToRiskReport(reportData, OutputDirectory);
                }

                if (CheckPolicies)
                {
                    PolicyStatus policyStatus = new PolicyStatus(GetPolicies());
                    LogPolicyViolations(policyStatus);
                }
            }
        }

        public bool IsExcluded()
        {

            if (String.IsNullOrWhiteSpace(ExcludedModules))
            {
                return false;
            }
            else
            {
                ISet<string> excludedSet = new HashSet<string>();
                string[] projectNameArray = this.ExcludedModules.Split(new char[] { ',' });
                foreach (string projectName in projectNameArray)
                {
                    excludedSet.Add(projectName.Trim());
                }
                return excludedSet.Contains(HubProjectName.Trim());
            }
        }

        public HubServerConfig BuildHubServerConfig()
        {
            HubCredentials credentials = new HubCredentials(HubUsername, HubPassword);
            HubCredentials proxyCredentials = new HubCredentials(HubProxyUsername, HubProxyPassword);
            HubProxyInfo proxyInfo = new HubProxyInfo(HubProxyHost, HubProxyPort, proxyCredentials);
            HubServerConfig hubServerConfig = new HubServerConfig(HubUrl, HubTimeout, credentials, proxyInfo);
            return hubServerConfig;
        }

        #region Make Flat Dependency List

        public List<string> CreateFlatList()
        {
            // Load the packages.config file into a list of Packages
            NuGet.PackageReferenceFile configFile = new NuGet.PackageReferenceFile(PackagesConfigPath);
            List<NuGet.PackageReference> packages = new List<NuGet.PackageReference>(configFile.GetPackageReferences());
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();

            List<string> externalIds = new List<string>();
            foreach (NuGet.PackageReference packageReference in packages)
            {
                string externalId = bdioPropertyHelper.CreateNugetExternalId(packageReference.Id, packageReference.Version.ToString());
                externalIds.Add(externalId);
            }
            return externalIds;
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
            List<PackageMetadataResource> metadataResourceList = new List<PackageMetadataResource>();
            metadataResourceList.Add(packageMetadataResource);

            // Create BDIO
            BdioContent bdioContent = BuildBOMFromMetadata(new List<NuGet.PackageReference>(configFile.GetPackageReferences()), metadataResourceList);
            return bdioContent;
        }

        private List<PackageMetadataResource> CreateMetaDataResourceList(List<Lazy<INuGetResourceProvider>> providers)
        {
            List<PackageMetadataResource> list = new List<PackageMetadataResource>();
            string[] splitRepoUrls = PackagesRepoUrl.Split(new char[]{','});

            foreach (string repoUrl in splitRepoUrls)
            {
                string url = repoUrl.Trim();
                if (!String.IsNullOrWhiteSpace(url))
                {
                    PackageSource packageSource = new PackageSource(url);
                    SourceRepository sourceRepository = new SourceRepository(packageSource, providers);
                    PackageMetadataResource packageMetadataResource = sourceRepository.GetResource<PackageMetadataResource>();
                    list.Add(packageMetadataResource);
                }
            }

            return list;
        }

        public BdioContent BuildBOMFromMetadata(List<NuGet.PackageReference> packages, List<PackageMetadataResource> metadataResourceList)
        {
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioNodeFactory bdioNodeFactory = new BdioNodeFactory(bdioPropertyHelper);
            BdioContent bdio = new BdioContent();

            // Create bdio bill of materials node
            BdioBillOfMaterials bdioBillOfMaterials = bdioNodeFactory.CreateBillOfMaterials(HubCodeLocationName, HubProjectName, HubVersionName);

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
                List<PackageDependency> packageDependencies = GetPackageDependencies(packageRef, metadataResourceList);
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

        public List<PackageDependency> GetPackageDependencies(NuGet.PackageReference packageDependency, List<PackageMetadataResource> metadataResourceList)
        {
            List<PackageDependency> dependencies = new List<PackageDependency>();
            foreach(PackageMetadataResource metadataResource in metadataResourceList)
            {
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

        private void WaitForHub()
        {
            ScanStatusDataService.AssertBomImportScanStartedThenFinished(HubProjectName, HubVersionName);
        }

        private void WaitForScanComplete(HttpClient client, HubPagedResponse<ScanSummaryView> currentPagedSummaries)
        {
            int currentSummaries = 0;
            if (currentPagedSummaries != null)
            {
                currentSummaries = currentPagedSummaries.TotalCount;
            }

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

            ScanStatusEnum currentStatus = ScanStatusEnum.UNSTARTED;
            while (stopwatch.ElapsedMilliseconds / 1000 < HubScanTimeout)
            {
                HubPagedResponse<ScanSummaryView> scanSummaries = ScanSummariesDataService.GetScanSummaries(codeLocation);
                if (scanSummaries == null || scanSummaries.Items == null)
                {
                    throw new BlackDuckIntegrationException($"There are no scan summaries @: {codeLocation.Metadata.Href}");
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
                throw new BlackDuckIntegrationException($"Scanning of the codelocation: {codeLocation.Metadata.Href} execeded the {HubScanTimeout} second timeout");
            }
        }

        #endregion

        #region Create Policies

        public VersionBomPolicyStatusView GetPolicies()
        {
            ProjectView project = ProjectDataService.GetProjectView(HubProjectName);
            ProjectVersionView projectVersion = ProjectDataService.GetMostRecentVersion(project);
            VersionBomPolicyStatusView policyStatus = PolicyDataService.GetVersionBomPolicyStatusView(projectVersion);
            return policyStatus;
        }

        private void LogPolicyViolations(PolicyStatus policyStatus)
        {
            StringBuilder stringBuilder = new StringBuilder(100);
            stringBuilder.Append("The Hub found: ");
            stringBuilder.Append(policyStatus.InViolationCount);
            stringBuilder.Append(" components in violation, ");
            stringBuilder.Append(policyStatus.InViolationCount);
            stringBuilder.Append(" components in violation, but overridden, and ");
            stringBuilder.Append(policyStatus.NotInViolationCount);
            stringBuilder.Append(" components not in violation.");

            if (PolicyStatusEnum.IN_VIOLATION.Equals(policyStatus.OverallStatus))
            {
                string error = $"The Hub found: {policyStatus.InViolationCount} components in violation\n";
                throw new BlackDuckIntegrationException(error);
            }
        }
        #endregion
    }

    // For the NuGet API
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
            Trace.WriteLine($"INFORMATION SUMMARY: {data}");
        }

        public void LogErrorSummary(string data)
        {
            Trace.WriteLine($"ERROR SUMMARY: {data}");
        }
    }
}
