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
            BuildBOMTask task = new BuildBOMTask();
            task.HubProjectName = "BuildBomTask";
            task.HubVersionName = "0.0.1";
            task.ProjectPath = "C:/Users/Black_Duck/Documents/Visual Studio 2015/Projects/hub-nuget/BuildBomTask";
            task.PackagesConfigPath = $"{task.ProjectPath}/packages.config";
            task.ProjectFilePath = $"{task.ProjectPath}/BuildBomTask.csproj";
            task.PackagesRepoPath = $"{task.ProjectPath}/../packages";
            task.Execute();
        }
    }
}
