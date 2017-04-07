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

        public override bool Execute()
        {
            bool result = true;
            string originalOutputDirectory = OutputDirectory;
            string originalHubProjectName = HubProjectName;
            string originalHubVersionName = HubVersionName;
            try
            {
                // TODO: clean up this code to generate the BDIO first then perform the deploy and checks for each project
                // Also aggregate the results of the check policies.
                Dictionary<string, string> projectData = ParseSolutionFile(SolutionPath);
                Console.WriteLine("Parsed Solution File");
                if (projectData.Count > 0)
                {
                    string solutionDirectory = Path.GetDirectoryName(SolutionPath);
                    Console.WriteLine("Solution directory: {0}", solutionDirectory);
                    foreach (string key in projectData.Keys)
                    {
                        OutputDirectory = originalOutputDirectory + Path.DirectorySeparatorChar + key;
                        string projectRelativePath = projectData[key];
                        List<string> projectPathSegments = new List<string>();
                        projectPathSegments.Add(solutionDirectory);
                        projectPathSegments.Add(projectRelativePath);

                        ProjectPath = CreatePath(projectPathSegments);

                        if (string.IsNullOrWhiteSpace(originalHubProjectName))
                        {
                            HubProjectName = key;
                        }

                        if (String.IsNullOrWhiteSpace(originalHubVersionName))
                        {
                            HubVersionName = originalHubVersionName;
                        }

                        bool projectResult = base.Execute();
                        PackagesConfigPath = ""; // reset to use the project packages file.
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
    }
}
