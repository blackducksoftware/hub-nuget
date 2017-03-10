using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using NUnit.Framework;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    class DeployTest
    {
        private BuildBOMTask Task = new BuildBOMTask();

        private HubServerConfig HubServerConfig;
        private RestConnection RestConnection;

        private CodeLocationDataService CodeLocationDataService;
        private ScanSummariesDataService ScanSummariesDataService;

        private string BdioId;
        private int PreScanCount = 0;

        [OneTimeSetUp]
        public void Setup()
        {
            HubNugetTestConfig.ConfigureTask(Task);
            Task.Setup();

            // Configure task properties
            Task.CreateFlatDependencyList = false;
            Task.CreateHubBdio = true;
            Task.DeployHubBdio = true;
            Task.CheckPolicies = false;
            Task.CreateHubReport = false;

            Task.WaitForDeployment = true;

            // Setup hub connection
            HubServerConfig = Task.BuildHubServerConfig();
            RestConnection = new CredentialsResetConnection(HubServerConfig);

            // Intialize data Services
            CodeLocationDataService = new CodeLocationDataService(RestConnection);
            ScanSummariesDataService = new ScanSummariesDataService(RestConnection);

            // Determine preconditions
            PreScanCount = GetScanCount();

            Task.Execute();
        }

        [Test]
        public void Deploy_SuccessTest()
        {
            Assert.Greater(GetScanCount(), PreScanCount);
        }

        private int GetScanCount()
        {
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioId = bdioPropertyHelper.CreateBdioId(Task.HubProjectName, Task.HubVersionName);
            CodeLocationView codeLocationView = CodeLocationDataService.GetCodeLocationView(BdioId);
            if (codeLocationView != null)
            {
                return ScanSummariesDataService.GetScanSummaries(codeLocationView).TotalCount;
            }
            return 0;
        }
    }
}
