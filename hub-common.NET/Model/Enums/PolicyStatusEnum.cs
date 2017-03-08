
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class PolicyStatusEnum : HubEnum
    {
        public static readonly PolicyStatusEnum IN_VIOLATION = new PolicyStatusEnum("IN_VIOLATION");
        public static readonly PolicyStatusEnum IN_VIOLATION_OVERRIDDEN = new PolicyStatusEnum("IN_VIOLATION_OVERRIDDEN");
        public static readonly PolicyStatusEnum NOT_IN_VIOLATION = new PolicyStatusEnum("NOT_IN_VIOLATION");

        public PolicyStatusEnum()
        {

        }

        public PolicyStatusEnum(string value) : base(value)
        {
        }
    }
}
