using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using Com.Blackducksoftware.Integration.Hub.Nuget;
using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Api;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    public class BuildBomTest
    {

        HubServerConfig HubServerConfig;
        private BuildBOMTask task = new BuildBOMTask();
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
            task.CreateFlatDependencyList = false;
            task.CreateHubBdio = false;
            task.DeployHubBdio = false;

            using (HttpClient client = task.CreateClient(HubServerConfig))
            {
                oldScanCount = task.GetCurrentScanSummaries(client).Result;
            }

            // Deploy resources
            File.WriteAllLines($"{task.OutputDirectory}/packages.config", Resources.packages.Split('\n'));

            // Run task
            task.Execute();

            // Generate Report

            // Check policies
        }

        [Test]
        public void Other()
        {
            using(RestConnection restConnection = task.CreateClient(HubServerConfig))
            {
                CodeLocationDataService codeLocationDS = new CodeLocationDataService(restConnection);
                List<CodeLocationView> codeLocations = codeLocationDS.FetchCodeLocations();
                foreach(CodeLocationView codeLocation in codeLocations)
                {
                    Console.WriteLine(codeLocation.Json);
                }
            }
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
            BdioContent expected = BuildBOMTask.ParseBdio(Resources.sample_bdio);
            BdioContent actual = BuildBOMTask.ParseBdio(actualString);
            actual.BillOfMaterials.Id = "uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5";
            // Change UUID to match the sample file
            Assert.AreEqual(expected, actual);
            Console.WriteLine($"EXPECTED\n{expected}\nACTUAL\n{actual}");
        }

        [Test]
        public void DeploymentTest()
        {
            PageScanSummaryView scanSummaries = null;
            using (HttpClient client = task.CreateClient(HubServerConfig))
            {
                PageCodeLocationView codeLocations = task.CodeLocationsAPI(client).Result;
                if (codeLocations.TotalCount > 0)
                {
                    CodeLocationView codeLocation = codeLocations.Items[0];
                    string codeLocationId = codeLocation.Metadata.GetFirstId(codeLocation.Metadata.Href);
                    scanSummaries = task.ScanSummariesAPI(client, codeLocationId).Result;
                }
            }
            Assert.IsNotNull(scanSummaries);
            Assert.Greater(scanSummaries.TotalCount, oldScanCount);
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
