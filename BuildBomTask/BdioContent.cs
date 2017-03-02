using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    public class BdioContent
    {
        public BdioBillOfMaterials BillOfMaterials { get; set; }
        public BdioProject Project { get; set; }
        public List<BdioNode> Components { get; set; } = new List<BdioNode>();
        public int Count
        {
            get {
                int count = 0;
                if (BillOfMaterials != null)
                {
                    count++;
                }
                if (Project != null)
                {
                    count++;
                }
                if (Components != null)
                {
                    count += Components.Count;
                }
                return count;
            }
        }

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

        public override bool Equals(object obj)
        {
            var other = obj as BdioContent;
            if (other == null)
                return false;
            bool bom = BillOfMaterials.Equals(other.BillOfMaterials);
            bool project = Project.Equals(other.Project);
            bool components = Components.Count == other.Components.Count;
            foreach(BdioComponent component in other.Components)
            {
                if(!Components.Contains(component))
                {
                    components = false;
                    break;
                }
            }
            return bom && project && components;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
