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

            StringBuilder stringBuilder = new StringBuilder();
            TextWriter textWriter = new StringWriter(stringBuilder);
            task.Writer = textWriter;
   
            task.Execute();

            VerifyJsonArraysEqual(Properties.Resources.sample, stringBuilder.ToString()); 
        }

        private void VerifyJsonArraysEqual(string expectedJson, string actualJson)
        {
            JArray expected = JArray.Parse(expectedJson);
            JArray actual = JArray.Parse(actualJson);

            Assert.AreEqual(expected.Count, actual.Count, string.Format("Expected count [{0}] \t Actual count [{1}]", expected.Count, actual.Count));

            foreach (JToken expectedToken in expectedJson)
            {
                bool found = false;
                foreach (JToken actualToken in actualJson)
                {
                    if (JToken.DeepEquals(expectedToken, actualToken))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Assert.IsTrue(false, string.Format("\n{0}\ndoes not exist in\n{1}", expectedToken.ToString(), expectedJson.ToString()));
                }
            }
        }
    }
}
