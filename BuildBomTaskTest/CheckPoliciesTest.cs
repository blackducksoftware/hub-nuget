using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
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

        private ProjectDataService ProjectDataService;
        private PolicyDataService PolicyDataService;

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
            ProjectDataService = new ProjectDataService(RestConnection);
            PolicyDataService = new PolicyDataService(RestConnection);

            Task.Execute();
        }

        [Test]
        public void CheckPolicy_ContentTest()
        {
            Project project = ProjectDataService.GetMostRecentProjectItem(Task.HubProjectName);
            VersionBomPolicyStatusView policyStatusView = PolicyDataService.GetVersionBomPolicyStatusView(project.ProjectId, project.VersionId);
            PolicyStatus policyStatus = new PolicyStatus(policyStatusView);

            Assert.AreEqual(HubNugetTestConfig.OVERALL_STATUS, policyStatus.OverallStatus);
            Assert.AreEqual(HubNugetTestConfig.COMPONENT_IN_VIOLATION, policyStatus.InViolationCount);
            Assert.AreEqual(HubNugetTestConfig.COMPONENT_IN_VIOLATION_OVERRIDEN, policyStatus.InViolationOverriddenCount);
            Assert.AreEqual(HubNugetTestConfig.COMPONENT_NOT_IN_VIOLATION, policyStatus.NotInViolationCount);
        }
    }
}

