using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    public class SolutionBomTest
    {
        private SolutionBuildBOMTask task = new SolutionBuildBOMTask();

        [OneTimeSetUp]
        public void ExecuteTaskTest()
        {
            task.HubProjectName = "SolutionBuildBomTask";
            task.HubVersionName = "0.0.1";
            task.OutputDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}/output";
            task.PackagesConfigPath = $"{task.OutputDirectory}/packages.config";
            task.PackagesRepoPath = $"https://api.nuget.org/v3/index.json";
            task.HubUrl = "http://int-hub01.dc1.lan:8080";
            task.HubUsername = "sysadmin";
            task.HubPassword = "blackduck";
            task.SolutionPath = "";

            // Run task
            task.Execute();
        }

    }
}
