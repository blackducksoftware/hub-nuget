using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Constants;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    public class BuildBomTest
    {

        HubServerConfig HubServerConfig;
        private BuildBOMTask task = new BuildBOMTask();
        private string BdioId;
        private int oldScanCount;

        [OneTimeSetUp]
        public void ExecuteTaskTest()
        {
            task.HubProjectName = "BuildBomTask";
            task.HubVersionName = "0.0.1";
            task.OutputDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}/output";
            task.PackagesConfigPath = $"{task.OutputDirectory}/packages.config";
            task.PackagesRepoPath = $"https://api.nuget.org/v3/index.json";
            task.HubUrl = "http://int-hub01.dc1.lan:8080";
            task.HubUsername = "sysadmin";
            task.HubPassword = "blackduck";

            // Server setup
            HubCredentials credentials = new HubCredentials(task.HubUsername, task.HubPassword);
            HubCredentials proxyCredentials = new HubCredentials(task.HubProxyUsername, task.HubProxyPassword);
            HubProxyInfo proxyInfo = new HubProxyInfo(task.HubProxyHost, task.HubProxyPort, proxyCredentials);
            HubServerConfig = new HubServerConfig(task.HubUrl, task.HubTimeout, credentials, proxyInfo);

            // Task options
            task.CreateFlatDependencyList = true;
            task.CreateHubBdio = true;
            task.DeployHubBdio = true;
            task.CheckPolicies = true;

            // Initialize helper properties
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioId = bdioPropertyHelper.CreateBdioId(task.HubProjectName, task.HubVersionName);
            task.BdioId = BdioId;

            task.RestConnection = new CredentialsResetConnection(HubServerConfig);
            task.CodeLocationDataService = new CodeLocationDataService(task.RestConnection);
            task.ScanSummariesDataService = new ScanSummariesDataService(task.RestConnection);
            CodeLocationView codeLocationView = task.CodeLocationDataService.GetCodeLocationView(task.BdioId);
            oldScanCount = 0;
            if (codeLocationView != null)
            {
                oldScanCount = task.ScanSummariesDataService.GetScanSummaries(codeLocationView).TotalCount;
            }

            // Deploy resources
            Directory.CreateDirectory(task.OutputDirectory);
            File.WriteAllText($"{task.OutputDirectory}/packages.config", Resources.packages);

            // Run task
            task.Execute();
        }

        [Test]
        public void FlatListTest()
        {
            List<string> expectedFlatList = new List<string>(Resources.sample_flat.Split('\n'));
            List<string> actualFlatList = new List<string>(File.ReadAllLines($"{task.OutputDirectory}/{task.HubProjectName}_flat.txt", Encoding.UTF8));
            Assert.AreEqual(expectedFlatList.Count, actualFlatList.Count);
            foreach (string actual in actualFlatList)
            {
                bool found = false;
                foreach (string expected in expectedFlatList)
                {
                    if (expected.Trim().Equals(actual.Trim()))
                    {
                        found = true;
                        break;
                    }
                }
                Assert.IsTrue(found, $"\n{actual} \nNOT FOUND IN\n{Resources.sample_flat}");
            }
            Console.WriteLine("\nEXPECTED");
            Console.WriteLine(Resources.sample_flat);
            Console.WriteLine("\nACTUAL");
            WriteArrayToConsole(actualFlatList.ToArray());
        }

        [Test]
        public void BuildBOMTest()
        {
            string actualString = File.ReadAllText($"{task.OutputDirectory}/{task.HubProjectName}.jsonld");
            BdioContent expected = BdioContent.Parse(Resources.sample_bdio);
            BdioContent actual = BdioContent.Parse(actualString);
            actual.BillOfMaterials.Id = "uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5";
            // Change UUID to match the sample file
            Assert.AreEqual(expected, actual);
            Console.WriteLine($"EXPECTED\n{expected}\nACTUAL\n{actual}");
        }

        [Test]
        public void DeploymentTest()
        {
            HubPagedResponse<ScanSummaryView> scanSummaries = null;
            CodeLocationDataService codeLocationDS = new CodeLocationDataService(task.RestConnection);
            CodeLocationView codeLocation = codeLocationDS.GetCodeLocationView(BdioId);
            Assert.IsNotNull(codeLocation);

            scanSummaries = task.ScanSummariesDataService.GetScanSummaries(codeLocation);
            Assert.IsNotNull(scanSummaries);
            Assert.Greater(scanSummaries.TotalCount, oldScanCount);
        }

        [Test]
        public void PolicyCheckTest()
        {
            Console.WriteLine(task.GetPolicies().Json);
            PolicyStatus policyStatus = new PolicyStatus(task.GetPolicies());
            Assert.AreEqual(PolicyStatusEnum.NOT_IN_VIOLATION, policyStatus.OverallStatus);
            Assert.AreEqual(0, policyStatus.InViolationCount);
            Assert.AreEqual(0, policyStatus.InViolationOverriddenCount);
            Assert.AreEqual(25, policyStatus.NotInViolationCount);
        }

        private void WriteArrayToConsole(object[] objects)
        {
            foreach (object item in objects)
            {
                Console.WriteLine(item);
            }
        }
    }
}
