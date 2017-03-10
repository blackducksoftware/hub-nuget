using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Nuget.Properties;
using Microsoft.Build.Evaluation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    public class SolutionBomTest
    {
        private const string PROJECT_NAME_SAMPLE_1 = "sample_project_1";
        private const string PROJECT_NAME_SAMPLE_2 = "sample_project_2";
        private const string PROJECT_NAME_SAMPLE_SOLUTION = "sample_solution";

        private List<string> projectNameList = new List<string>();
        private SolutionBuildBOMTask task = new SolutionBuildBOMTask();
        private HubServerConfig HubServerConfig;

        [OneTimeSetUp]
        public void ExecuteTaskTest()
        {
            projectNameList.Add(PROJECT_NAME_SAMPLE_1);
            projectNameList.Add(PROJECT_NAME_SAMPLE_2);
            projectNameList.Add(PROJECT_NAME_SAMPLE_SOLUTION);

            task.HubUrl = "http://www.blackducksoftware.com";
            task.HubUsername = "auser";
            task.HubPassword = "apassword";
            task.PackagesRepoUrl = $"https://api.nuget.org/v3/index.json";
            task.SolutionPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}/resources/sample_solution/sample_solution.sln";
            // Server setup
            HubCredentials credentials = new HubCredentials(task.HubUsername, task.HubPassword);
            HubCredentials proxyCredentials = new HubCredentials(task.HubProxyUsername, task.HubProxyPassword);
            HubProxyInfo proxyInfo = new HubProxyInfo(task.HubProxyHost, task.HubProxyPort, proxyCredentials);
            HubServerConfig = new HubServerConfig(task.HubUrl, task.HubTimeout, credentials, proxyInfo);

            //task.RestConnection = new CredentialsResetConnection(HubServerConfig);
            //task.CodeLocationDataService = new CodeLocationDataService(task.RestConnection);
            //task.ScanSummariesDataService = new ScanSummariesDataService(task.RestConnection);
        }

        [Test]
        public void Solution_BuildBOMTest()
        {
            task.OutputDirectory = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}/output";
            task.DeployHubBdio = false;

            // Deploy resources
            Directory.CreateDirectory(task.OutputDirectory);
            task.Execute();

            foreach (string projectName in projectNameList)
            {
                string actualString = File.ReadAllText($"{task.OutputDirectory}/{projectName}.jsonld");
                BdioContent expected = BdioContent.Parse(Resources.old_sample_bdio);
                BdioContent actual = BdioContent.Parse(actualString);

                Assert.AreEqual(expected.Count, actual.Count);
                // bill of materials check
                Assert.AreEqual($"{projectName} Black Duck I/O Export", actual.BillOfMaterials.Name);
                // project check
                Assert.AreEqual($"{projectName}", actual.Project.Name);
                Assert.AreEqual($"data:{projectName}/1.0.0.0", actual.Project.Id);
                Assert.AreEqual("nuget", actual.Project.BdioExternalIdentifier.Forge);
                Assert.AreEqual($"{projectName}/1.0.0.0", actual.Project.BdioExternalIdentifier.ExternalId);
                // check component list
                List<BdioNode> expectedComponents = expected.Components;
                List<BdioNode> actualComponents = actual.Components;
                foreach (BdioNode node in actualComponents)
                {
                    Assert.IsTrue(expectedComponents.Contains(node));
                }
            }
        }

        [Test]
        public void Solution_BuildBOMInProjectDirectoryTest()
        {
            task.DeployHubBdio = false;
            task.OutputDirectory = null;
            task.Execute();

            var loadedProjects = ProjectCollection.GlobalProjectCollection.LoadedProjects;
            foreach (string projectName in projectNameList)
            {
                string outputPath = null;
                string projectFullPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\\resources\\sample_solution\\{projectName}\\{projectName}.csproj";
                var projectList = ProjectCollection.GlobalProjectCollection.LoadedProjects.Where(item => item.GetPropertyValue("ProjectName").Equals(projectName));
                Project project;
                if (projectList.Count() > 0)
                {
                    project = projectList.First();
                }
                else
                {
                    project = new Project(projectFullPath);
                }
                if (project != null)
                {
                    ICollection<ProjectProperty> propertyGroup = project.AllEvaluatedProperties;
                    var outputPathProperties = propertyGroup.Where(property => property.Name.Equals("OutputPath"));
                    string builddirectory = outputPathProperties.First().EvaluatedValue;
                    outputPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\\resources\\sample_solution\\{projectName}{Path.DirectorySeparatorChar}{builddirectory}";
                }
                string actualString = File.ReadAllText($"{outputPath}\\{projectName}.jsonld");
                BdioContent expected = BdioContent.Parse(Resources.old_sample_bdio);
                BdioContent actual = BdioContent.Parse(actualString);

                Assert.AreEqual(expected.Count, actual.Count);
                // bill of materials check
                Assert.AreEqual($"{projectName} Black Duck I/O Export", actual.BillOfMaterials.Name);
                // project check
                Assert.AreEqual($"{projectName}", actual.Project.Name);
                Assert.AreEqual($"data:{projectName}/1.0.0.0", actual.Project.Id);
                Assert.AreEqual("nuget", actual.Project.BdioExternalIdentifier.Forge);
                Assert.AreEqual($"{projectName}/1.0.0.0", actual.Project.BdioExternalIdentifier.ExternalId);
                // check component list
                List<BdioNode> expectedComponents = expected.Components;
                List<BdioNode> actualComponents = actual.Components;
                foreach (BdioNode node in actualComponents)
                {
                    Assert.IsTrue(expectedComponents.Contains(node));
                }
            }
        }

        [Test]
        public void Solution_FlatListProjectDirectoryTest()
        {
            task.CreateFlatDependencyList = true;
            task.DeployHubBdio = false;
            task.OutputDirectory = null;
            task.Execute();

            foreach (string projectName in projectNameList)
            {
                string outputPath = null;
                string projectFullPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\\resources\\sample_solution\\{projectName}\\{projectName}.csproj";
                var projectList = ProjectCollection.GlobalProjectCollection.LoadedProjects.Where(item => item.GetPropertyValue("ProjectName").Equals(projectName));
                Project project;
                if (projectList.Count() > 0)
                {
                    project = projectList.First();
                }
                else
                {
                    project = new Project(projectFullPath);
                }
                if (project != null)
                {
                    ICollection<ProjectProperty> propertyGroup = project.AllEvaluatedProperties;
                    var outputPathProperties = propertyGroup.Where(property => property.Name.Equals("OutputPath"));
                    string builddirectory = outputPathProperties.First().EvaluatedValue;
                    outputPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}\\resources\\sample_solution\\{projectName}{Path.DirectorySeparatorChar}{builddirectory}";
                }
                List<string> expectedFlatList = new List<string>(Resources.old_sample_flat.Split('\n'));
                List<string> actualFlatList = new List<string>(File.ReadAllLines($"{outputPath}\\{projectName}_flat.txt", Encoding.UTF8));
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
                    Assert.IsTrue(found, $"\n{actual} \nNOT FOUND IN\n{Resources.old_sample_flat}");
                }
                Console.WriteLine("\nEXPECTED");
                Console.WriteLine(Resources.old_sample_flat);
                Console.WriteLine("\nACTUAL");
                Console.WriteLine(File.ReadAllText($"{outputPath}\\{projectName}_flat.txt", Encoding.UTF8));
            }
        }
    }
}
