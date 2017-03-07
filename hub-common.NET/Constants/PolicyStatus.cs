
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Constants
{
    public sealed class PolicyStatus
    {
        private readonly string Status;

        public static readonly PolicyStatus IN_VIOLATION = new PolicyStatus("IN_VIOLATION");
        public static readonly PolicyStatus IN_VIOLATION_OVERRIDDEN = new PolicyStatus("IN_VIOLATION_OVERRIDDEN");
        public static readonly PolicyStatus NOT_IN_VIOLATION = new PolicyStatus("NOT_IN_VIOLATION");

        public PolicyStatus(string statusName)
        {
            Status = statusName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PolicyStatus;
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
