
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class CodeLocationTypeEnum : HubEnum
    {
        public static readonly CodeLocationTypeEnum SCM = new CodeLocationTypeEnum("SCM");
        public static readonly CodeLocationTypeEnum FS = new CodeLocationTypeEnum("FS");
        public static readonly CodeLocationTypeEnum BOM_IMPORT = new CodeLocationTypeEnum("BOM_IMPORT");

        public CodeLocationTypeEnum()
        {

        }

        public CodeLocationTypeEnum(string value) : base(value)
        {
        }
    }
}
