using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    class FlatDependencyListTest
    {
        private BuildBOMTask Task = new BuildBOMTask();

        private string FlatListFilePath;

        [OneTimeSetUp]
        public void Setup()
        {
            HubNugetTestConfig.ConfigureTask(Task);

            FlatListFilePath = $"{Task.OutputDirectory}/{Task.HubProjectName}_flat.txt";

            // Configure task properties
            Task.CreateFlatDependencyList = true;
            Task.CreateHubBdio = false;
            Task.DeployHubBdio = false;
            Task.CheckPolicies = false;
            Task.CreateHubReport = false;

            Task.Execute();
        }

        [Test, Order(1)]
        public void FlatList_ExistanceTest()
        {
            DirectoryAssert.Exists(Task.OutputDirectory);
            FileAssert.Exists(FlatListFilePath);
        }

        [Test]
        public void FlatList_ContentTest()
        {
            List<string> expectedFlatList = new List<string>(Resources.sample_flat.Split('\n'));
            string actualFlatListFile = File.ReadAllText(FlatListFilePath, Encoding.UTF8).Trim();
            List<string> actualFlatList = new List<string>(actualFlatListFile.Split('\n'));

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
            Console.WriteLine(actualFlatListFile);
        }
    }
}
