using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using System;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    class HubNugetTestConfig
    {
        // Overall
        public const int COMPONENT_TOTAL_COUNT = 27;
        public const int COMPONENT_MATCHED_COUNT = 26;

        // Policy
        public static PolicyStatusEnum OVERALL_STATUS = PolicyStatusEnum.NOT_IN_VIOLATION;
        public const int COMPONENT_IN_VIOLATION = 0;
        public const int COMPONENT_IN_VIOLATION_OVERRIDEN = 0;
        public const int COMPONENT_NOT_IN_VIOLATION = COMPONENT_MATCHED_COUNT;

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
