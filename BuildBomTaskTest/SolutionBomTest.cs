using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using NUnit.Framework;
using System;
using System.IO;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    public class SolutionBomTest
    {
        private SolutionBuildBOMTask task = new SolutionBuildBOMTask();

        [OneTimeSetUp]
        public void ExecuteTaskTest()
        {
            task.OutputDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}/output";
            task.HubUrl = "http://int-hub01.dc1.lan:8080";
            task.HubUsername = "sysadmin";
            task.HubPassword = "blackduck";
            task.SolutionPath = $"{AppDomain.CurrentDomain.BaseDirectory}/resources/sample_solution/sample_solution.sln";

            // Deploy resources
            Directory.CreateDirectory(task.OutputDirectory);
            File.WriteAllLines($"{task.OutputDirectory}/packages.config", Resources.packages.Split('\n'));

            // Run task
            task.Execute();
        }

        [Test]
        public void SolutionBuildBOMTest()
        {
            task.DeployHubBdio = false;
            /*
            string actualString = File.ReadAllText($"{task.OutputDirectory}/{task.HubProjectName}.jsonld");
            BdioContent expected = BdioContent.Parse(Resources.sample_bdio);
            BdioContent actual = BdioContent.Parse(actualString);
            actual.BillOfMaterials.Id = "uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5";
            // Change UUID to match the sample file
            Assert.AreEqual(expected, actual);
            Console.WriteLine($"EXPECTED\n{expected}\nACTUAL\n{actual}");
            */
        }
    }
}
