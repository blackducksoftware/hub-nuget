
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants
{
    class VersionBomPolicyStatusOverallStatusEnum
    {
        private readonly string OverallStatus;

        public static readonly VersionBomPolicyStatusOverallStatusEnum IN_VIOLATION = new VersionBomPolicyStatusOverallStatusEnum("IN_VIOLATION");
        public static readonly VersionBomPolicyStatusOverallStatusEnum IN_VIOLATION_OVERRIDDEN = new VersionBomPolicyStatusOverallStatusEnum("IN_VIOLATION_OVERRIDDEN");
        public static readonly VersionBomPolicyStatusOverallStatusEnum NOT_IN_VIOLATION = new VersionBomPolicyStatusOverallStatusEnum("NOT_IN_VIOLATION");

        public VersionBomPolicyStatusOverallStatusEnum(string overallStatus)
        {
            OverallStatus = overallStatus;
        }

        public override bool Equals(object obj)
        {
            var other = obj as VersionBomPolicyStatusOverallStatusEnum;
            if (other == null)
                return false;
            return OverallStatus == other.OverallStatus;
        }

        public override int GetHashCode()
        {
            return OverallStatus.GetHashCode();
        }

        public override string ToString()
        {
            return OverallStatus;
        }
    }
}
