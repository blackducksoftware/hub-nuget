
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Constants
{
    class LicenseType
    {
        private readonly string Type;

        public static readonly LicenseType CONJUNCTIVE = new LicenseType("CONJUNCTIVE");
        public static readonly LicenseType DISJUNCTIVE = new LicenseType("DISJUNCTIVE");

        public LicenseType(string type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LicenseType;
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
