using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Text;
using System.IO;

namespace com.blackducksoftware.integration.hub.nuget
{
    [TestFixture]
    public class BuildBomTest
    {
        [Test]
        public void ExecuteTaskTest()
        {
            BuildBOMTask task = new BuildBOMTask();
            task.HubProjectName = "BuildBomTask";
            task.HubVersionName = "0.0.1";
            task.ProjectPath = "C:/Users/Black_Duck/Documents/Visual Studio 2015/Projects/hub-nuget/BuildBomTask";
            task.PackagesConfigPath = $"{task.ProjectPath}/packages.config";
            task.ProjectFilePath = $"{task.ProjectPath}/BuildBomTask.csproj";
            task.PackagesRepoPath = $"{task.ProjectPath}/../packages";
            task.HubUrl = "http://int-hub01.dc1.lan:8080";
            task.HubUsername = "sysadmin";
            task.HubPassword = "blackduck";

            BdioContent bdioContent = task.BuildBOM();
            bdioContent.BillOfMaterials.Id = "uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5";

            System.Threading.Tasks.Task deployTask = task.Deploy(bdioContent);
            deployTask.Wait();

            VerifyJsonArraysEqual(Properties.Resources.sample, bdioContent.ToString()); 
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
    }
}
