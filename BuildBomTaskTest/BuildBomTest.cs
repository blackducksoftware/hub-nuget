using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    public class BuildBomTest
    {

        BuildBOMTask task = new BuildBOMTask();

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

            // Task options
            task.CreateFlatDependencyList = true;

            // Deploy resources
            File.WriteAllLines($"{task.OutputDirectory}/packages.config", Resources.packages.Split('\n'));

            // Run task
            task.Execute();

            // Building BOM
            /*
            BdioContent bdioContent = task.BuildBOM();
            bdioContent.BillOfMaterials.Id = "uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5";
            VerifyJsonArraysEqual(Properties.Resources.sample, bdioContent.ToString());

            // Deploy BOM to hub
            System.Threading.Tasks.Task deployTask = task.Deploy(bdioContent);
            deployTask.GetAwaiter().GetResult();
            //*/

            // Generate Report

            // Check policies
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

        private void VerifyJsonArraysEqual(string expectedString, string actualString)
        {
            JArray expected = JArray.Parse(expectedString);
            JArray actual = JArray.Parse(actualString);

            Assert.AreEqual(expected.Count, actual.Count, string.Format("Expected count [{0}] \t Actual count [{1}]", expected.Count, actual.Count));

            foreach (JToken expectedToken in expected)
            {
                bool found = false;
                foreach (JToken actualToken in actual)
                {
                    if (JToken.DeepEquals(expectedToken, actualToken))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Assert.IsTrue(false, string.Format("\n{0}\ndoes not exist in\n{1}", expectedToken, actual));
                }
            }
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
