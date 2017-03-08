
namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Model.Enums
{
    public class HubEnum
    {
        private readonly string Value;

        public HubEnum() {
            Value = null;
        }

        public HubEnum(string value)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            var other = obj as HubEnum;
            if (other == null)
                return false;
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
