﻿
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Constants
{
    class LicenseTypeEnum
    {
        private readonly string Type;

        public static readonly LicenseTypeEnum CONJUNCTIVE = new LicenseTypeEnum("CONJUNCTIVE");
        public static readonly LicenseTypeEnum DISJUNCTIVE = new LicenseTypeEnum("DISJUNCTIVE");

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