namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Global
{
    public class HubCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public HubCredentials()
        {

        }

        public HubCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
