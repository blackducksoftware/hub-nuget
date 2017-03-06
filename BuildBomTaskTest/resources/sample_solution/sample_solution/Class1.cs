using Com.Blackducksoftware.Integration.Hub.Nuget.Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.Sample
{
    public class Class1
    {
        public Class1() : base()
        {
            SampleClass1 class1 = new SampleClass1();
            SampleClass2 class2 = new SampleClass2();
            Console.WriteLine("Created objects.");
        }
        
        public static void Main(string[] args)
        {
            Class1 newinstance = new Class1();
            Console.WriteLine("Sample main method executed.");
        }
    }
    
}
