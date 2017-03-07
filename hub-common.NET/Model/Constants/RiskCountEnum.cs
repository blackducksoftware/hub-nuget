
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants
{
    public sealed class RiskCountEnum
    {
        private readonly string Type;

        public static readonly RiskCountEnum UNKNOWN = new RiskCountEnum("UNKNOWN");
        public static readonly RiskCountEnum OK = new RiskCountEnum("OK");
        public static readonly RiskCountEnum LOW = new RiskCountEnum("LOW");
        public static readonly RiskCountEnum MEDIUM = new RiskCountEnum("MEDIUM");
        public static readonly RiskCountEnum HIGH = new RiskCountEnum("HIGH");

        public RiskCountEnum(string type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as RiskCountEnum;
            if (other == null)
                return false;
            return Type == other.Type;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override string ToString()
        {
            return Type;
        }
    }
}
