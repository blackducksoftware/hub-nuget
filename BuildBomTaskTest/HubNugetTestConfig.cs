using System;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    class HubNugetTestConfig
    {
        public static void ConfigureTask(BuildBOMTask task)
        {
            task.HubProjectName = "BuildBomTask";
            task.HubVersionName = "0.0.1";
            task.OutputDirectory = $"{AppDomain.CurrentDomain.BaseDirectory}/output";
            task.PackagesConfigPath = $"{AppDomain.CurrentDomain.BaseDirectory}/../../../BuildBomTask/packages.config";
            task.PackagesRepoUrl = $"https://api.nuget.org/v3/index.json";
            task.HubUrl = "http://int-hub01.dc1.lan:8080";
            task.HubUsername = "sysadmin";
            task.HubPassword = "blackduck";
        }
    }
}
