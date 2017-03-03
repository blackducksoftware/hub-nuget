using System;
using System.IO;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Utilities;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    public class SolutionBuildBOMTask : BuildBOMTask
    {
        [Required]
        public string SolutionPath { get; set; }

        public override bool Execute()
        {
            bool result = false;
            try
            {
                Dictionary<string,string> projectData = ParseSolutionFile(SolutionPath);
                if(!projectData.Empty())
                {
                    string solutionDirectory = Path.GetDirectoryName(solutionPath);
                    foreach(string key in projectData)
                    {
                        string projectRelativePath = projectData[key];
                        List<string> projectPathSegments = new List<string>();
                        projectPathSegments.add(solutionDirectory);
                        projectPathSegments.add(Path.GetDirectoryName(projectRelativePath));

                        HubProjectName = key;
                        HubVersionName = GetProjectAssemblyVersion(projectPathSegments);
                        PackagesConfigPath = CreateProjectPackageConfigPath(projectPathSegments);
                        OutputDirectory = CreateOutputDirectoryPath(solutionDirectory,projectRelativePath);
                        bool projectResult = base.execute();
                        if(projectResult)
                        {
                            Console.WriteLine("Generated Bdio file for project {0}",projectData[key]);
                        }
                        else
                        {
                            Console.WriteLine("Could not generate Bdio file for project {0}",projectData[key]);
                        }
                        result = result && projectResult;
                    }
                }
                else
                {
                    Console.WriteLine("No project data found for solution {0}",SolutionPath);
                }
            }
            catch(Exception ex)
            {
                if(HubIgnoreFailure)
                {
                    result = true;
                    Console.WriteLine("Error occurred building Black Duck I/O file for solution: {0}",ex);
                }
                else
                {   result = false;
                    throw new BlackDuckIntegrationException(ex);
                }
            }

            return result;
        }

        private string CreatePath(List<string> pathSegments)
        {
            return String.Join(Path.PathSeparator,pathSegments);
        }

        private Project CreateProjectObject(string solutionDirectory, string projectRelativePath)
        {
            Engine buildEngine = Engine.GlobalEngine;
            Project project = new Project(buildEngine);
            List<string> pathSegments = new List<string>();
            pathSegments.add(solutionDirectory);
            pathSegments.add(projectRelativePath);
            project.Load(CreatePath(pathSegments));

            return project;
        }

        private Dictionary<string,string> ParseSolutionFile(string solutionPath)
        {
            Dictionary<string,string> projectDataMap = new Dictionary<string,string>();
            // Visual Studio right now is not resolving the Microsoft.Build.Construction.SolutionFile type
            // parsing the solution file manually for now.
            if(File.Exists(solutionPath))
            {
                List<string> contents = new List<string>(File.ReadAllLines(solutionPath));
                var projectLines = contents.FindAll(text => text.StartsWith("Project("));
                foreach(string projectText in projectLines)
                {
                    int equalIndex = projectText.IndexOf("=");
                    if(equalIndex > -1)
                    {
                        string projectValuesCSV = projectText.Substring(equalIndex);
                        projectValues.ReplaceAll("\"","");
                        string[] projectValues = projectValuesCSV.Split(",");

                        if(projectValues.Length >= 2)
                        {
                            projectDataMap.put(projectValues[0].Trim(), projectValues[1].Trim());
                        }
                    }
                }
                Console.WriteLine("Found {0} Projects elements, processed {1} project elements for data",projectLines.Count(), projectDataMap.Count());
            }
            else
            {
                throw new BlackDuckIntegrationException("Solution File "+ solutionPath +" not found");
            }

            return projectDataMap;
        }

        private string GetProjectAssemblyVersion(List<string> projectPathSegments)
        {
            string version = DateTime.Now().ToString("yyyy-MM-dd_HH-mm-ss");
            List<string> pathSegments = new List<string>(projectPathSegments);
            pathSegments.add("Properties");
            pathSegments.add("AssemblyInfo.cs");
            string path = CreatePath(pathSegments);

            if(File.Exists(path))
            {
                string[] contents = File.ReadAllLines(path);
                var versionText = contents.( text => text.Contains("[assembly: AssemblyVersion"));
                foreach(string text in versionText)
                {
                    int firstParen = text.IndexOf("(");
                    int lastParen = text.LastIndexOf(")");
                    // exclude the '(' and the " characters
                    int start = firstParen + 2;
                    // exclude the ')' and the " characters
                    int end = lastParen - 2;
                    version = item.Substring(start, end);
                }
            }
            return version;
        }
        private string CreateProjectPackageConfigPath(List<string> projectPathSegments)
        {
            List<string> pathSegments = new List<string>(projectPathSegments);
            pathSegments.add("packages.config");
            return CreatePath(pathSegments);
        }

        private string CreateOutputDirectoryPath(string solutionDirectory, string projectRelativePath)
        {
            if(String.IsNotNullOrEmpty(OutputDirectory))
            {
                return OutputDirectory;
            }
            else
            {
                Project project = CreateProjectObject(solutionDirectory, projectRelativePath);
                if(project != null)
                {
                    BuildPropertyGroup propertyGroup = project.EvaluatedProperties;
                    string builddirectory = propertyGroup.Item["OutputPath"];

                    List<string> pathSegments = new List<string>();
                    pathSegments.add(solutionDirectory);
                    pathSegments.add(builddirectory);
                    return CreatePath(pathSegments);
                }
                else
                {
                    // shouldn't get here
                }
            }
        }
    }
}
