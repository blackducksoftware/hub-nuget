using System;
//using Microsoft.Build.Construction;
using Microsoft.Build.Utilities;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    public class SolutionBuildBOMTask : BuildBOMTask
    {
        /*
        [Required]
        public string SolutionPath { get; set; }

        public override bool Execute()
        {
            try
            {
                var solutionFile = SolutionFile.Parse(SolutionPath);
                var projectList = solutionFile.ProjectsInOrder;

                foreach(ProjectsInSolution project in projectList)
                {
                    HubProjectName=project.ProjectName;
                    HubVersionName= GetProjectAssemblyVersion(project);
                    PackagesConfigPath=project.AbsolutePath+"\packages.config";
                    if(base.Execute())
                    {
                        Console.WriteLine("Generated Bdio file for project {0}",)project.ProjectName);
                    }
                    else
                    {
                        Console.WriteLine("Could not generate Bdio file for project {0}",)project.ProjectName);
                    }
                }
            }
            catch(Exception ex)
            {
                if(HubIgnoreFailure)
                {
                    Console.WriteLine("Error Occurred build Black Duck I/O file for solution: {0}",ex);
                }
                else
                {
                    throw new BlackDuckIntegrationException(ex);
                }
            }
        }

        public string GetProjectAssemblyVersion(ProjectsInSolution project)
        {
            string version = DateTime.Now().ToString("yyyy-MM-dd_HH-mm-ss");
            string path = project.AbsolutePath+"\Properties\AssemblyInfo.cs";

            if(File.Exists(path))
            {
                string[] contents = File.ReadAllLines(path);
                var versionText = contents.Where( text => text.Contains("[assembly: AssemblyVersion"));
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
        */
    }
}
