
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public class TrendingEnum : HubEnum
    {
        public static readonly TrendingEnum DECREASING = new TrendingEnum("DECREASING");
        public static readonly TrendingEnum STABLE = new TrendingEnum("STABLE");
        public static readonly TrendingEnum INCREASING = new TrendingEnum("INCREASING");

        public TrendingEnum()
        {

        }

        public TrendingEnum(string value) : base(value)
        {
        }
    }
}
