using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using NUnit.Framework;
using System;
using System.IO;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    class BdioGenerationTest
    {
        private BuildBOMTask Task = new BuildBOMTask();

        private string BdioFilePath;

        [OneTimeSetUp]
        public void Setup()
        {
            HubNugetTestConfig.ConfigureTask(Task);

            BdioFilePath = $"{Task.OutputDirectory}/{Task.HubProjectName}.jsonld";

            // Configure task properties
            Task.CreateFlatDependencyList = false;
            Task.CreateHubBdio = true;
            Task.DeployHubBdio = false;
            Task.CheckPolicies = false;
            Task.CreateHubReport = false;

            Task.Execute();
        }

        [Test, Order(1)]
        public void Bdio_ExistanceTest()
        {
            DirectoryAssert.Exists(Task.OutputDirectory);
            FileAssert.Exists(BdioFilePath);
        }

        [Test]
        public void Bdio_ContentTest()
        {
            string actualString = File.ReadAllText(BdioFilePath);
            BdioContent expected = BdioContent.Parse(Resources.sample_bdio);
            BdioContent actual = BdioContent.Parse(actualString);
            // Change UUID to match the sample file
            actual.BillOfMaterials.Id = "uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5";
       
            Assert.AreEqual(expected, actual);

            Console.WriteLine($"EXPECTED\n{expected}\nACTUAL\n{actual}");
        }
    }
}
