using Com.Blackducksoftware.Integration.Hub.Common.Net.Api.ResponseService;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.CodeLocation;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Global;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Project;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Model.ScanStatus;
using Com.Blackducksoftware.Integration.Hub.Common.Net.Rest;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.DataService
{
    public class ScanStatusDataService : HubResponseService
    {
        private static int FIVE_SECONDS = 5 * 1000;
        private static long DEFAULT_TIMEOUT = 300000;
        
        private static ISet<ScanStatusEnum> PENDING_STATES = new HashSet<ScanStatusEnum>() { ScanStatusEnum.UNSTARTED, ScanStatusEnum.SCANNING,
                        ScanStatusEnum.SAVING_SCAN_DATA, ScanStatusEnum.SCAN_DATA_SAVE_COMPLETE, ScanStatusEnum.REQUESTED_MATCH_JOB, ScanStatusEnum.MATCHING,
                        ScanStatusEnum.BOM_VERSION_CHECK, ScanStatusEnum.BUILDING_BOM };

        private static ISet<ScanStatusEnum> DONE_STATES = new HashSet<ScanStatusEnum>() { ScanStatusEnum.COMPLETE, ScanStatusEnum.CANCELLED,
                    ScanStatusEnum.CLONED, ScanStatusEnum.ERROR_SCANNING, ScanStatusEnum.ERROR_SAVING_SCAN_DATA, ScanStatusEnum.ERROR_MATCHING,
                    ScanStatusEnum.ERROR_BUILDING_BOM, ScanStatusEnum.ERROR };

        private static ISet<ScanStatusEnum> ERROR_STATES = new HashSet<ScanStatusEnum>() { ScanStatusEnum.CANCELLED, ScanStatusEnum.ERROR_SCANNING,
                    ScanStatusEnum.ERROR_SAVING_SCAN_DATA, ScanStatusEnum.ERROR_MATCHING, ScanStatusEnum.ERROR_BUILDING_BOM, ScanStatusEnum.ERROR };

        private long timeoutInMilliseconds;

        private ProjectResponseService projectDataService;
        private ProjectVersionResponseService projectVersionDataService;
        private CodeLocationResponseService codeLocationDataService;
        private ScanSummariesResponseService scanSummaryDataService;

        public ScanStatusDataService(RestConnection restConnection, long timeoutInMilliseconds) : base(restConnection)
        {
            projectDataService = new ProjectResponseService(restConnection);
            projectVersionDataService = new ProjectVersionResponseService(restConnection);
            codeLocationDataService = new CodeLocationResponseService(restConnection);
            scanSummaryDataService = new ScanSummariesResponseService(restConnection);

            long timeout = timeoutInMilliseconds;
            if (timeoutInMilliseconds <= 0)
            {
                timeout = DEFAULT_TIMEOUT;
            }
            this.timeoutInMilliseconds = timeout;
        }

        public bool IsPending(ScanStatusEnum statusEnum)
        {
            return PENDING_STATES.Contains(statusEnum);
        }

        public bool IsDone(ScanStatusEnum statusEnum)
        {
            return DONE_STATES.Contains(statusEnum);
        }

        public bool IsError(ScanStatusEnum statusEnum)
        {
            return ERROR_STATES.Contains(statusEnum);
        }

        public void AssertBomImportScanStartedThenFinished(String projectName, String projectVersion)
        {
            List<ScanSummaryView> pendingScans = WaitForPendingScansToStart(projectName, projectVersion,
                    timeoutInMilliseconds);
            WaitForScansToComplete(pendingScans, timeoutInMilliseconds);
        }

        public void AssertBomImportScansFinished(List<ScanSummaryView> pendingScans)
        {
            WaitForScansToComplete(pendingScans, timeoutInMilliseconds);
        }

        private List<ScanSummaryView> WaitForPendingScansToStart(String projectName, String projectVersion, long scanStartedTimeoutInMilliseconds)
        {
            List<ScanSummaryView> pendingScans = GetPendingScans(projectName, projectVersion);
            long startedTime = GetCurrentTimeInMillis();
            bool pendingScansOk = pendingScans.Count > 0;
            while (!Done(pendingScansOk, scanStartedTimeoutInMilliseconds, startedTime,
                    "No scan has started within the specified wait time: {0} minutes"))
            {
                try
                {
                    Thread.Sleep(FIVE_SECONDS);
                }
                catch (ArgumentOutOfRangeException e) {
                    throw new BlackDuckIntegrationException("The thread waiting for the scan to start was interrupted: ", e);
                }
                pendingScans = GetPendingScans(projectName, projectVersion);
                pendingScansOk = pendingScans.Count > 0;
            }

            return pendingScans;
        }

        private void WaitForScansToComplete(List<ScanSummaryView> pendingScans, long scanStartedTimeoutInMilliseconds)
        {
            pendingScans = GetPendingScans(pendingScans);
            long startedTime = GetCurrentTimeInMillis();
            bool pendingScansOk = pendingScans.Count == 0;
            while (!Done(pendingScansOk, scanStartedTimeoutInMilliseconds, startedTime,
                    "The pending scans have not completed within the specified wait time: {0} minutes"))
            {
                
                try {
                    Thread.Sleep(FIVE_SECONDS);
                } catch (ArgumentOutOfRangeException e) {
                    throw new BlackDuckIntegrationException("The thread waiting for the scan to complete was interrupted: ", e);
                }
                pendingScans = GetPendingScans(pendingScans);
                pendingScansOk = pendingScans.Count == 0;
            }
        }

        private bool Done(bool pendingScansOk, long timeoutInMilliseconds, long startedTime, string timeoutMessage)
        {
            if (pendingScansOk) {
                return true;
            }

            if (TakenTooLong(timeoutInMilliseconds, startedTime)) {
                throw new BlackDuckIntegrationException(
                        String.Format(timeoutMessage, TimeSpan.FromMilliseconds(timeoutInMilliseconds).TotalMinutes));
            }

            return false;
        }

        private bool TakenTooLong(long timeoutInMilliseconds, long startedTime)
        {
            long elapsed = GetCurrentTimeInMillis() - startedTime;
            return elapsed > timeoutInMilliseconds;
        }

        private long GetCurrentTimeInMillis()
        {
            return Convert.ToInt64(new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds);
        }

        private List<ScanSummaryView> GetPendingScans(String projectName, String projectVersion)
        {
            List<ScanSummaryView> pendingScans = new List<ScanSummaryView>();
            try
            {
                ProjectView projectItem = projectDataService.GetProjectView(projectName);
                ProjectVersionView projectVersionItem = projectVersionDataService.GetProjectVersion(projectItem, projectVersion);
                string projectVersionUrl = projectVersionItem.Metadata.Href;

                List<CodeLocationView> allCodeLocations = codeLocationDataService.GetAllCodeLocationsForCodeLocationType(CodeLocationTypeEnum.BOM_IMPORT);
                List<string> allScanSummariesLinks = new List<string>();
                foreach (CodeLocationView codeLocationItem in allCodeLocations)
                {
                    string mappedProjectVersionUrl = codeLocationItem.MappedProjectVersion;
                    if (projectVersionUrl.Equals(mappedProjectVersionUrl))
                    {
                        string scanSummariesLink = MetadataResponseService.GetLink(codeLocationItem, ApiLinks.SCANS_LINK);
                        allScanSummariesLinks.Add(scanSummariesLink);
                    }
                }

                List<ScanSummaryView> allScanSummaries = new List<ScanSummaryView>();
                foreach (string scanSummaryLink in allScanSummariesLinks)
                {
                    allScanSummaries.AddRange(scanSummaryDataService.GetAllItems<ScanSummaryView>(scanSummaryLink));
                }

                pendingScans = new List<ScanSummaryView>();
                foreach (ScanSummaryView scanSummaryItem in allScanSummaries)
                {
                    if (IsPending(scanSummaryItem.Status))
                    {
                        pendingScans.Add(scanSummaryItem);
                    }
                }
            }
            catch (Exception ex)
            {
                pendingScans = new List<ScanSummaryView>();
                // ignore, since we might not have found a project or version, etc
                // so just keep waiting until the timeout
            }

            return pendingScans;
        }

        private List<ScanSummaryView> GetPendingScans(List<ScanSummaryView> scanSummaries)
        {
            List<ScanSummaryView> pendingScans = new List<ScanSummaryView>();
            foreach (ScanSummaryView scanSummaryItem in scanSummaries) {
                string scanSummaryLink = scanSummaryItem.Metadata.Href;
                ScanSummaryView currentScanSummaryItem = scanSummaryDataService.GetItem<ScanSummaryView>(scanSummaryLink);
                if (IsPending(currentScanSummaryItem.Status)) {
                    pendingScans.Add(currentScanSummaryItem);
                } else if (IsError(currentScanSummaryItem.Status)) {
                    throw new BlackDuckIntegrationException("There was a problem in the Hub processing the scan(s). Error Status : "
                            + currentScanSummaryItem.Status.ToString());
                }
            }

            return pendingScans;
        }
    }
}
