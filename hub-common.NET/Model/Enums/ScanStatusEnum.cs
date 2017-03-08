
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class ScanStatusEnum : HubEnum
    {
        public static readonly ScanStatusEnum UNSTARTED = new ScanStatusEnum("UNSTARTED");
        public static readonly ScanStatusEnum SCANNING = new ScanStatusEnum("SCANNING");
        public static readonly ScanStatusEnum SAVING_SCAN_DATA = new ScanStatusEnum("SAVING_SCAN_DATA");
        public static readonly ScanStatusEnum SCAN_DATA_SAVE_COMPLETE = new ScanStatusEnum("SCAN_DATA_SAVE_COMPLETE");
        public static readonly ScanStatusEnum REQUESTED_MATCH_JOB = new ScanStatusEnum("REQUESTED_MATCH_JOB");
        public static readonly ScanStatusEnum MATCHING = new ScanStatusEnum("MATCHING");
        public static readonly ScanStatusEnum BOM_VERSION_CHECK = new ScanStatusEnum("BOM_VERSION_CHECK");
        public static readonly ScanStatusEnum BUILDING_BOM = new ScanStatusEnum("BUILDING_BOM");
        public static readonly ScanStatusEnum COMPLETE = new ScanStatusEnum("COMPLETE");
        public static readonly ScanStatusEnum CANCELLED = new ScanStatusEnum("CANCELLED");
        public static readonly ScanStatusEnum CLONED = new ScanStatusEnum("CLONED");
        public static readonly ScanStatusEnum ERROR_SCANNING = new ScanStatusEnum("ERROR_SCANNING");
        public static readonly ScanStatusEnum ERROR_SAVING_SCAN_DATA = new ScanStatusEnum("ERROR_SAVING_SCAN_DATA");
        public static readonly ScanStatusEnum ERROR_MATCHING = new ScanStatusEnum("ERROR_MATCHING");
        public static readonly ScanStatusEnum ERROR_BUILDING_BOM = new ScanStatusEnum("ERROR_BUILDING_BOM");
        public static readonly ScanStatusEnum ERROR = new ScanStatusEnum("ERROR");

        public ScanStatusEnum()
        {

        }

        public ScanStatusEnum(string value) : base(value)
        {
        }
    }
}
