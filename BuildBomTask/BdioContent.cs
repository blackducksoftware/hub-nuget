using com.blackducksoftware.integration.hub.bdio.simple;
using com.blackducksoftware.integration.hub.bdio.simple.model;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.blackducksoftware.integration.hub.nuget
{
    public class BdioContent
    {
        public BdioBillOfMaterials BillOfMaterials { get; set; }
        public BdioProject Project { get; set; }
        public List<BdioNode> Components { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);
            using (BdioWriter bdioWriter = new BdioWriter(stringWriter))
            {
                bdioWriter.WriteBdioNode(BillOfMaterials);
                bdioWriter.WriteBdioNode(Project);
                bdioWriter.WriteBdioNodes(Components);
            }
            return stringBuilder.ToString();
        }
    }
}
