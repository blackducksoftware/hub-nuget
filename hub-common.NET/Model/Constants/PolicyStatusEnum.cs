
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants
{
    public sealed class PolicyStatusEnum
    {
        private readonly string Status;

        public static readonly PolicyStatusEnum IN_VIOLATION = new PolicyStatusEnum("IN_VIOLATION");
        public static readonly PolicyStatusEnum IN_VIOLATION_OVERRIDDEN = new PolicyStatusEnum("IN_VIOLATION_OVERRIDDEN");
        public static readonly PolicyStatusEnum NOT_IN_VIOLATION = new PolicyStatusEnum("NOT_IN_VIOLATION");

        public PolicyStatusEnum(string statusName)
        {
            Status = statusName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PolicyStatusEnum;
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
