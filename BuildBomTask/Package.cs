using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace com.blackducksoftware.integration.hub.nuget
{
    public class Package
    {
        [XmlElement(ElementName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "targetFramework")]
        public string TargetFramework { get; set; }
    }
}
