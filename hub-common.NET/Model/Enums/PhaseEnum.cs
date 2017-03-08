
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class PhaseEnum : HubEnum
    {
        public static readonly PhaseEnum PLANNING = new PhaseEnum("PLANNING");
        public static readonly PhaseEnum DEVELOPMENT = new PhaseEnum("DEVELOPMENT");
        public static readonly PhaseEnum RELEASED = new PhaseEnum("RELEASED");
        public static readonly PhaseEnum DEPRECATED = new PhaseEnum("DEPRECATED");
        public static readonly PhaseEnum ARCHIVED = new PhaseEnum("ARCHIVED");

        public PhaseEnum()
        {

        }

        public PhaseEnum(string value) : base(value)
        {
        }
    }
}
