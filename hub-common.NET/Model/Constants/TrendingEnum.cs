
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants
{
    public class TrendingEnum
    {
        private readonly string Type;

        public static readonly TrendingEnum DECREASING = new TrendingEnum("DECREASING");
        public static readonly TrendingEnum STABLE = new TrendingEnum("STABLE");
        public static readonly TrendingEnum INCREASING = new TrendingEnum("INCREASING");

        public TrendingEnum(string type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TrendingEnum;
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
