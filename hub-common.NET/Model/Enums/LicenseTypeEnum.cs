
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class LicenseTypeEnum:HubEnum
    {
        public static readonly LicenseTypeEnum CONJUNCTIVE = new LicenseTypeEnum("CONJUNCTIVE");
        public static readonly LicenseTypeEnum DISJUNCTIVE = new LicenseTypeEnum("DISJUNCTIVE");

        public LicenseTypeEnum()
        {

        }

        public LicenseTypeEnum(string value) : base(value)
        {
        }
    }
}
