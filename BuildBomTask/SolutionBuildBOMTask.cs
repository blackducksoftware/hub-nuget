using System;
using Microsoft.Build.Utilities;

namespace Com.Blackducksoftware.Integration.Hub.Nuget
{
    public class SolutionBuildBOMTask : BuildBOMTask
    {
        public override bool Execute()
        {
            // for each project set the properties and call the base execute method.
            return base.Execute();           
        }
    }
}
