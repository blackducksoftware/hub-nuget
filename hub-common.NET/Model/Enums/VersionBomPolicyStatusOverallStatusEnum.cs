
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public class VersionBomPolicyStatusOverallStatusEnum : HubEnum
    {
        public static readonly VersionBomPolicyStatusOverallStatusEnum IN_VIOLATION = new VersionBomPolicyStatusOverallStatusEnum("IN_VIOLATION");
        public static readonly VersionBomPolicyStatusOverallStatusEnum IN_VIOLATION_OVERRIDDEN = new VersionBomPolicyStatusOverallStatusEnum("IN_VIOLATION_OVERRIDDEN");
        public static readonly VersionBomPolicyStatusOverallStatusEnum NOT_IN_VIOLATION = new VersionBomPolicyStatusOverallStatusEnum("NOT_IN_VIOLATION");

        public VersionBomPolicyStatusOverallStatusEnum()
        {
        }

        public VersionBomPolicyStatusOverallStatusEnum(string value) : base(value)
        {
        }
    }
}
