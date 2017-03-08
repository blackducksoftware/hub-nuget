
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class RiskCountEnum : HubEnum
    {
        public static readonly RiskCountEnum UNKNOWN = new RiskCountEnum("UNKNOWN");
        public static readonly RiskCountEnum OK = new RiskCountEnum("OK");
        public static readonly RiskCountEnum LOW = new RiskCountEnum("LOW");
        public static readonly RiskCountEnum MEDIUM = new RiskCountEnum("MEDIUM");
        public static readonly RiskCountEnum HIGH = new RiskCountEnum("HIGH");

        public RiskCountEnum()
        {

        }

        public RiskCountEnum(string value) : base(value)
        {
        }
    }
}
