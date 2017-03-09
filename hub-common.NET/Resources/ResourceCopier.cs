using System.Net;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Resource
{
    public class ResourceCopier
    {

        public ResourceCopier()
        {

        }

        public void CopyFromWeb(string url, string filePath)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, filePath);
        }
    }


}
