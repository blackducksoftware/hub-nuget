using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Constants
{
    public sealed class ScanStatus
    {
        private readonly string Status;

        public static readonly ScanStatus UNSTARTED = new ScanStatus("UNSTARTED");
        public static readonly ScanStatus SCANNING = new ScanStatus("SCANNING");
        public static readonly ScanStatus SAVING_SCAN_DATA = new ScanStatus("SAVING_SCAN_DATA");
        public static readonly ScanStatus SCAN_DATA_SAVE_COMPLETE = new ScanStatus("SCAN_DATA_SAVE_COMPLETE");
        public static readonly ScanStatus REQUESTED_MATCH_JOB = new ScanStatus("REQUESTED_MATCH_JOB");
        public static readonly ScanStatus MATCHING = new ScanStatus("MATCHING");
        public static readonly ScanStatus BOM_VERSION_CHECK = new ScanStatus("BOM_VERSION_CHECK");
        public static readonly ScanStatus BUILDING_BOM = new ScanStatus("BUILDING_BOM");
        public static readonly ScanStatus COMPLETE = new ScanStatus("COMPLETE");
        public static readonly ScanStatus CANCELLED = new ScanStatus("CANCELLED");
        public static readonly ScanStatus CLONED = new ScanStatus("CLONED");
        public static readonly ScanStatus ERROR_SCANNING = new ScanStatus("ERROR_SCANNING");
        public static readonly ScanStatus ERROR_SAVING_SCAN_DATA = new ScanStatus("ERROR_SAVING_SCAN_DATA");
        public static readonly ScanStatus ERROR_MATCHING = new ScanStatus("ERROR_MATCHING");
        public static readonly ScanStatus ERROR_BUILDING_BOM = new ScanStatus("ERROR_BUILDING_BOM");
        public static readonly ScanStatus ERROR = new ScanStatus("ERROR");

        public ScanStatus(string statusName)
        {
            Status = statusName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ScanStatus;
            if (other == null)
                return false;
            return Status == other.Status;
        }

        public override int GetHashCode()
        {
            return Status.GetHashCode();
        }

        public override string ToString()
        {
            return Status;
        }
    }
}
