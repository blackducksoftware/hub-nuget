using com.blackducksoftware.integration.hub.bdio.simple.model;
using System.Collections.Generic;

namespace com.blackducksoftware.integration.hub.nuget
{
    public class Bdio
    {
        public BdioBillOfMaterials BillOfMaterials { get; set; }
        public BdioProject Project { get; set; }
        public List<BdioNode> Components { get; set; }
    }
}
