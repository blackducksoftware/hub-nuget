using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using NUnit.Framework;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    class CheckPoliciesTest
    {
        private BuildBOMTask Task = new BuildBOMTask();

        private HubServerConfig HubServerConfig;
        private RestConnection RestConnection;

        private CodeLocationDataService CodeLocationDataService;
        private ScanSummariesDataService ScanSummariesDataService;

        private string BdioId;

        [OneTimeSetUp]
        public void Setup()
        {
            HubNugetTestConfig.ConfigureTask(Task);

            // Configure task properties
            Task.CreateFlatDependencyList = false;
            Task.CreateHubBdio = true;
            Task.DeployHubBdio = true;
            Task.CheckPolicies = true;
            Task.CreateHubReport = false;

            Task.WaitForDeployment = true;

            // Setup hub connection
            HubServerConfig = Task.BuildHubServerConfig();
            RestConnection = new CredentialsResetConnection(HubServerConfig);

            // Intialize data Services
            CodeLocationDataService = new CodeLocationDataService(RestConnection);
            ScanSummariesDataService = new ScanSummariesDataService(RestConnection);

            //Task.Execute();
        }

        [Test]
        public void CheckPolicy_CountTest()
        {

        }

        [Test]
        public void CheckPolicy_ContentTest()
        {

        }
    }
}
