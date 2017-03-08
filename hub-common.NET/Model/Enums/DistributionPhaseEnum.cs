
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public sealed class DistributionEnum : HubEnum
    {
        public static readonly DistributionEnum EXTERNAL = new DistributionEnum("EXTERNAL");
        public static readonly DistributionEnum SAAS = new DistributionEnum("SAAS");
        public static readonly DistributionEnum INTERNAL = new DistributionEnum("INTERNAL");
        public static readonly DistributionEnum OPENSOURCE = new DistributionEnum("OPENSOURCE");

        public DistributionEnum()
        {

        }

        public DistributionEnum(string value) : base(value)
        {
        }
    }
}
