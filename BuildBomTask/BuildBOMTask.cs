using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.ComponentModel;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Threading;
using NuGet.Protocol;
using NuGet.Protocol.Core.v2;
using NuGet.Configuration;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace com.blackducksoftware.integration.hub.nuget
{
    public class BuildBOMTask : Task
    {
        #region Hub Properties
        [Required]
        [DisplayName("hub.url")]
        public string HubUrl { get; set; }

        [Required]
        [DisplayName("hub.username")]
        public string HubUsername { get; set; }

        [Required]
        [DisplayName("hub.password")]
        public string HubPassword { get; set; }

        [DisplayName("hub.timeout")]
        public int HubTimeout { get; set; } = 120;
        [DisplayName("hub.project.name")]
        public string HubProjectName { get; set; }
        [DisplayName("hub.version.name")]
        public string HubVersionName { get; set; }
        #endregion

        #region Proxy Properties
        [DisplayName("hub.proxy.host")]
        public string HubProxyHost { get; set; }
        [DisplayName("hub.proxy.port")]
        public string HubProxyPort { get; set; }
        [DisplayName("hub.proxy.username")]
        public string HubProxyUsername { get; set; }
        [DisplayName("hub.proxy.password")]
        public string HubProxyPassword { get; set; }

        #endregion

        [DisplayName("hub.scan.timeout")]
        public int HubScanTimeout { get; set; } = 300;
        [DisplayName("hub.output.directory")]
        public string OutputDirectory { get; set; }
        // [DisplayName("included.scopes")]
        // public String IncludedScopes { get; set; }
        [DisplayName("excluded.modules")]
        public string ExcludedModules { get; set; } = "";
        [DisplayName("hub.ignore.failure")]
        public bool HubIgnoreFailure { get; set; } = false;
        [DisplayName("hub.create.flat.list")]
        public bool CreateFlatDependencyList { get; set; } = false;
        [DisplayName("hub.create.bdio")]
        public bool CreateHubBdio { get; set; } = false;
        [DisplayName("hub.deploy.bdio")]
        public bool DeployHubBdio { get; set; } = false;
        [DisplayName("hub.create.report")]
        public bool CreateHubReport { get; set; } = false;
        [DisplayName("hub.check.policies")]
        public bool CheckPolicies { get; set; } = false;

        public override bool Execute()
        {
            string projectPath = "C:/Users/Black_Duck/Documents/Visual Studio 2015/Projects/hub-nuget/";

            // Load the packages.config file into a list of Packages
            FileStream configFileStream = new FileStream($"{projectPath}/packages.config", FileMode.Open);
            XmlReader reader = XmlReader.Create(configFileStream);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Package>));
            List<Package> referencedPackages = (List<Package>) serializer.Deserialize(reader);
            configFileStream.Close();

            // Setup NuGet API
            // Snippets taken from https://daveaglick.com/posts/exploring-the-nuget-v3-libraries-part-2 with modifications
            List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
            providers.AddRange(Repository.Provider.GetCoreV3());  // Add v3 API support
            providers.AddRange(Repository.Provider.GetCoreV2());  // Add v2 API support
            PackageSource packageSource = new PackageSource(projectPath);
            SourceRepository sourceRepository = new SourceRepository(packageSource, providers);
            PackageMetadataResource packageMetadataResource = sourceRepository.GetResource<PackageMetadataResource>();

            // TODO: Go through all the packages in the referencesPackages list and start figuring out dependencies
            List<IPackageSearchMetadata> searchMetadata = new List<IPackageSearchMetadata>(packageMetadataResource.GetMetadataAsync("NuGet.Client", true, true, new Logger(), CancellationToken.None).Result);


            string searchMetadataJson = searchMetadata.ToJson();
            Console.WriteLine(JToken.Parse(searchMetadataJson));

            return true;
        }
    }

    public class Logger : NuGet.Common.ILogger
    {
        public void LogDebug(string data) => Trace.WriteLine($"DEBUG: {data}");
        public void LogVerbose(string data) => Trace.WriteLine($"VERBOSE: {data}");
        public void LogInformation(string data) => Trace.WriteLine($"INFORMATION: {data}");
        public void LogMinimal(string data) => Trace.WriteLine($"MINIMAL: {data}");
        public void LogWarning(string data) => Trace.WriteLine($"WARNING: {data}");
        public void LogError(string data) => Trace.WriteLine($"ERROR: {data}");
        public void LogSummary(string data) => Trace.WriteLine($"SUMMARY: {data}");

        public void LogInformationSummary(string data)
        {
            throw new NotImplementedException();
        }

        public void LogErrorSummary(string data)
        {
            throw new NotImplementedException();
        }
    }
}
