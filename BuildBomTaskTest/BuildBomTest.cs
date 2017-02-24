using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.blackducksoftware.integration.hub.nuget;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildBomTaskTest
{
    [TestClass]
    public class BuildBomTest
    {
        [TestMethod]
        //[ExpectedException(typeof(NotImplementedException))]
        public void ExecuteTaskTest()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            BuildBOMTask task = new BuildBOMTask();
            task.Execute();
        }
    }
}
