
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants
{
    class LicenseTypeEnum
    {
        private readonly string Type;

        public static readonly LicenseTypeEnum IN_VIOLATION = new LicenseTypeEnum("IN_VIOLATION");
        public static readonly LicenseTypeEnum IN_VIOLATION_OVERRIDDEN = new LicenseTypeEnum("IN_VIOLATION_OVERRIDDEN");
        public static readonly LicenseTypeEnum NOT_IN_VIOLATION = new LicenseTypeEnum("NOT_IN_VIOLATION");

        public LicenseTypeEnum(string type)
        {
            Type = type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as LicenseTypeEnum;
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
