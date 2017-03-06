using Com.Blackducksoftware.Integration.Hub.Common.Net.Model;
using Newtonsoft.Json;

namespace Com.Blackducksoftware.Integration.Hub.Common.Net.Items
{
    public class ProjectItem
    {

        public string ProjectId { get; private set; }
        public string VersionId { get; private set; }

        private ProjectView _ProjectView;
        public ProjectView ProjectView
        {
            get { return _ProjectView; }
            set
            {
                _ProjectView = value;
                ProjectId = value.Metadata.GetFirstId(value.Metadata.Href);
            }
        }

        private ProjectVersionView _ProjectVersionView;
        public ProjectVersionView ProjectVersionView
        {
            get
            {
                return _ProjectVersionView;
            }
            set
            {
                _ProjectVersionView = value;
                VersionId = value.Metadata.GetId(value.Metadata.Href, 1);
            }
        }
    }
}
