using Com.Blackducksoftware.Integration.Hub.Common.Net;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.BuildBom
{
    public class SolutionGenerator : ProjectGenerator
    {
        public string SolutionPath { get; set; }

        public bool Verbose { get; set; } = false;
        public override bool Execute()
        {
            bool result = true;
            string originalOutputDirectory = OutputDirectory;
            try
            {
                Dictionary<string, string> projectData = ParseSolutionFile(SolutionPath);
                Console.WriteLine("Parsed Solution File");
                if (projectData.Count > 0)
                {
                    string solutionDirectory = Path.GetDirectoryName(SolutionPath);

                    Console.WriteLine("Solution directory: {0}", solutionDirectory);
                    bool useProjectOutputDir = false;

                    if (String.IsNullOrWhiteSpace(OutputDirectory))
                    {
                        useProjectOutputDir = true;
                    }

                    foreach (string key in projectData.Keys)
                    {
                        OutputDirectory = originalOutputDirectory;
                        Console.WriteLine("Processing Project: {0}", key);
                        string projectRelativePath = projectData[key];

                        List<string> projectPathSegments = new List<string>();
                        projectPathSegments.Add(solutionDirectory);
                        projectPathSegments.Add(Path.GetDirectoryName(projectRelativePath));

                        HubProjectName = key;
                        HubVersionName = GetProjectAssemblyVersion(projectPathSegments);
                        PackagesConfigPath = CreateProjectPackageConfigPath(projectPathSegments);

                        if (useProjectOutputDir) // create 
                        {
                            OutputDirectory = CreateOutputDirectoryPath(solutionDirectory, projectRelativePath);
                        }
                        else
                        {
                            OutputDirectory = originalOutputDirectory + Path.DirectorySeparatorChar + key;
                        }

                        bool projectResult = base.Execute();
                        if (projectResult)
                        {
                            Console.WriteLine("Generated Bdio file for project {0}", projectData[key]);
                        }
                        else
                        {
                            Console.WriteLine("Could not generate Bdio file for project {0}", projectData[key]);
                        }
                        result = result && projectResult;
                    }
                }
                else
                {
                    Console.WriteLine("No project data found for solution {0}", SolutionPath);
                }
            }
            catch (Exception ex)
            {
                if (HubIgnoreFailure)
                {
                    result = true;
                    Console.WriteLine("Error executing Build BOM task. Cause: {0}", ex);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                OutputDirectory = originalOutputDirectory; // reset the output directory to original path
            }

            return result;
        }

        private string CreatePath(List<string> pathSegments)
        {
            return String.Join(String.Format("{0}", Path.DirectorySeparatorChar), pathSegments);
        }

        private Project CreateProjectObject(string solutionDirectory, string projectRelativePath)
        {

            List<string> pathSegments = new List<string>();
            pathSegments.Add(solutionDirectory);
            pathSegments.Add(projectRelativePath);
            string projectFullPath = CreatePath(pathSegments);
            var projectList = ProjectCollection.GlobalProjectCollection.LoadedProjects.Where(item => item.FullPath.Equals(projectFullPath));
            Project project;
            if (projectList.Count() > 0)
            {
                project = projectList.First();
            }
            else
            {
                project = new Project(projectFullPath);
            }
            return project;
        }

        private Dictionary<string, string> ParseSolutionFile(string solutionPath)
        {
            Dictionary<string, string> projectDataMap = new Dictionary<string, string>();
            // Visual Studio right now is not resolving the Microsoft.Build.Construction.SolutionFile type
            // parsing the solution file manually for now.
            if (File.Exists(solutionPath))
            {
                List<string> contents = new List<string>(File.ReadAllLines(solutionPath));
                var projectLines = contents.FindAll(text => text.StartsWith("Project("));
                foreach (string projectText in projectLines)
                {
                    int equalIndex = projectText.IndexOf("=");
                    if (equalIndex > -1)
                    {
                        string projectValuesCSV = projectText.Substring(equalIndex + 1);
                        projectValuesCSV = projectValuesCSV.Replace("\"", "");
                        string[] projectValues = projectValuesCSV.Split(new char[] { ',' });

                        if (projectValues.Length >= 2)
                        {
                            projectDataMap[projectValues[0].Trim()] = projectValues[1].Trim();
                        }
                    }
                }
                Console.WriteLine("Black Duck I/O Generation Found {0} Project elements, processed {1} project elements for data", projectLines.Count(), projectDataMap.Count());
            }
            else
            {
                throw new BlackDuckIntegrationException("Solution File " + solutionPath + " not found");
            }

            return projectDataMap;
        }

        private string GetProjectAssemblyVersion(List<string> projectPathSegments)
        {
            string version = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            List<string> pathSegments = new List<string>(projectPathSegments);
            pathSegments.Add("Properties");
            pathSegments.Add("AssemblyInfo.cs");
            string path = CreatePath(pathSegments);

            if (File.Exists(path))
            {
                List<string> contents = new List<string>(File.ReadAllLines(path));
                var versionText = contents.FindAll(text => text.Contains("[assembly: AssemblyVersion"));
                foreach (string text in versionText)
                {
                    int firstParen = text.IndexOf("(");
                    int lastParen = text.LastIndexOf(")");
                    // exclude the '(' and the " characters
                    int start = firstParen + 2;
                    // exclude the ')' and the " characters
                    int end = lastParen - 1;
                    version = text.Substring(start, (end - start));
                }
            }
            return version;
        }

        private string CreateProjectPackageConfigPath(List<string> projectPathSegments)
        {
            List<string> pathSegments = new List<string>(projectPathSegments);
            pathSegments.Add("packages.config");
            return CreatePath(pathSegments);
        }

        private string CreateOutputDirectoryPath(string solutionDirectory, string projectRelativePath)
        {
            Project project = CreateProjectObject(solutionDirectory, projectRelativePath);
            if (project != null)
            {
                ICollection<ProjectProperty> propertyGroup = project.AllEvaluatedProperties;
                var outputPathProperties = propertyGroup.Where(property => property.Name.Equals("OutputPath"));
                string builddirectory = outputPathProperties.First().EvaluatedValue;
                List<string> pathSegments = new List<string>();
                pathSegments.Add(solutionDirectory);
                pathSegments.Add(Path.GetDirectoryName(projectRelativePath));
                pathSegments.Add(builddirectory);
                return CreatePath(pathSegments);
            }
            else
            {
                return null;
            }
        }
    }
}
