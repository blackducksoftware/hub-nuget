
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class SourceEnum : HubEnum
    {
        public static readonly SourceEnum CUSTOM = new SourceEnum("CUSTOM");
        public static readonly SourceEnum KB = new SourceEnum("KB");

        public SourceEnum()
        {

        }

        public SourceEnum(string value) : base(value)
        {
        }
    }
}
