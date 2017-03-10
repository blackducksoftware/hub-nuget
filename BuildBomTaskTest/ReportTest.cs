using Com.Blackducksoftware.Integration.Hub.Common.Net.Dataservices;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Items;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.PolicyStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using NUnit.Framework;
using System.IO;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    [TestFixture]
    class ReportTest
    {
        private BuildBOMTask Task = new BuildBOMTask();

        [OneTimeSetUp]
        public void Setup()
        {
            HubNugetTestConfig.ConfigureTask(Task);

            // Configure task properties
            Task.CreateFlatDependencyList = false;
            Task.CreateHubBdio = true;
            Task.DeployHubBdio = true;
            Task.CheckPolicies = true;
            Task.CreateHubReport = true;
        }

        [Test, Order(1)]
        public void Report_Execute()
        {
            Task.Execute();
        }

        [Test, Order(2)]
        public void Report_ExistanceTest()
        {
            DirectoryAssert.Exists($"{Task.OutputDirectory}/{RiskReportDataService.RISK_REPORT_DIRECTORY}");
            FileAssert.Exists($"{Task.OutputDirectory}/{RiskReportDataService.RISK_REPORT_DIRECTORY}/{RiskReportDataService.RISK_REPORT_HTML_FILE}");
        }

        public void Report_ContentTest()
        {
            string htmlFile = File.ReadAllText($"{Task.OutputDirectory}/{RiskReportDataService.RISK_REPORT_DIRECTORY}/{RiskReportDataService.RISK_REPORT_HTML_FILE}");
            bool replaced = !htmlFile.Contains(RiskReportDataService.REPLACEMENT_TOKEN);
            Assert.IsTrue(replaced, "The report html file still has its replacement token");
        }
    }
}

