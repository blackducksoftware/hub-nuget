
using Com.Blackducksoftware.Integration.Hub.Common.Net;
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.BuildBom
{
    public class Application
    {
        public const string PARAM_KEY_SOLUTION = "solution_path";
        public const string PARAM_KEY_HUB_URL = "hub_url";
        public const string PARAM_KEY_HUB_USERNAME = "hub_username";
        public const string PARAM_KEY_HUB_PASSWORD = "hub_password";
        public const string PARAM_KEY_PACKAGE_REPO_URL = "packages_repo_url";
        public const string PARAM_KEY_HUB_PROXY_HOST = "hub_proxy_host";
        public const string PARAM_KEY_HUB_PROXY_PORT = "hub_proxy_port";
        public const string PARAM_KEY_HUB_PROXY_USERNAME = "hub_proxy_username";
        public const string PARAM_KEY_HUB_PROXY_PASSWORD = "hub_proxy_password";
        public const string PARAM_KEY_HUB_PROJECT_NAME = "hub_project_name";
        public const string PARAM_KEY_HUB_VERSION_NAME = "hub_version_name";
        public const string PARAM_KEY_HUB_TIMEOUT = "hub_timeout";
        public const string PARAM_KEY_HUB_SCAN_TIMEOUT = "hub_scan_timeout";
        public const string PARAM_KEY_HUB_OUTPUT_DIRECTORY = "hub_output_directory";
        public const string PARAM_KEY_EXCLUDED_MODULES = "excluded_modules";
        public const string PARAM_KEY_HUB_IGNORE_FAILURE = "hub_ignore_failure";
        public const string PARAM_KEY_HUB_CREATE_FLAT_LIST = "hub_create_flat_list";
        public const string PARAM_KEY_HUB_CREATE_BDIO = "hub_create_bdio";
        public const string PARAM_KEY_HUB_DEPLOY_BDIO = "hub_deploy_bdio";
        public const string PARAM_KEY_HUB_CREATE_REPORT = "hub_create_report";
        public const string PARAM_KEY_HUB_CHECK_POLICIES = "hub_check_policies";
        public const string PARAM_KEY_HUB_CODE_LOCATION_NAME = "hub_code_location_name";

        private SolutionGenerator SolutionGenerator;
        private string[] Args;
        private Dictionary<string, string> PropertyMap;

        private bool ShowHelp = false;
        private bool Verbose = false;

        public Application(string [] args)
        {
            this.Args = args;
            PropertyMap = new Dictionary<string, string>();
            SolutionGenerator = CreateGenerator();
        }

        public static void Main(string[] args)
        {
            try
            {
                Application app = new Application(args);
                app.Execute();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        private SolutionGenerator CreateGenerator()
        {
            return new SolutionGenerator();
        }

        public bool Execute()
        {
            try
            {
                Configure();
                return SolutionGenerator.Execute();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error occurred executing command: {0}",ex.Message);
                Environment.Exit(-1);
                return false;
            }
        }

        private void Configure()
        {
            PopulateParameterMap();
            OptionSet commandOptions = CreateOptionSet();
            ParseCommandLine(commandOptions);
            ConfigureGenerator();
            
            if (Verbose)
            {
                Console.WriteLine("Configuration Properties: ");
                foreach (string key in PropertyMap.Keys)
                {
                    Console.WriteLine("Property {0} = {1}", key, PropertyMap[key]);
                }
            }

            if(ShowHelp)
            {
                ShowHelpMessage("Usage is BuildBom.exe --solution_path=[path to solution]", commandOptions);
            }
        }

        private void PopulateParameterMap()
        {
            NameValueCollection applicationProperties = ConfigurationManager.AppSettings;

            foreach (string key in applicationProperties.AllKeys)
            {
                PropertyMap[key] = applicationProperties[key];
            }
        }

        private OptionSet CreateOptionSet()
        {
            OptionSet optionSet = new OptionSet();

            AddMenuOption(optionSet, PARAM_KEY_SOLUTION, "The path to the solution file to find dependencies");
            AddMenuOption(optionSet, PARAM_KEY_HUB_URL, "The URL of the Hub to connect to.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_USERNAME, "The username to authenticate with the Hub.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_PASSWORD, "The password to authenticate with the Hub.");
            AddMenuOption(optionSet, PARAM_KEY_PACKAGE_REPO_URL, "The URL of the NuGet repository to get the packages.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_PROXY_HOST, "The host URL of the proxy server.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_PROXY_PORT, "The port of the proxy server.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_PROXY_USERNAME, "The username to authenticate with the proxy.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_PROXY_PASSWORD, "The password to authenticate with the proxy.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_PROJECT_NAME, "The name of the project in the Hub to map to.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_VERSION_NAME, "The version name for the project in the Hub to map to.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_TIMEOUT, "The timeout value for each http connection to the Hub.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_SCAN_TIMEOUT, "The time to wait for the Code Location to be updated in the Hub.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_OUTPUT_DIRECTORY, "The directory path to output the BDIO files.");
            AddMenuOption(optionSet, PARAM_KEY_EXCLUDED_MODULES, "The names of the projects in a solution to exclude from BDIO generation.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_IGNORE_FAILURE, "If true log the error but do not throw an exception.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_CREATE_FLAT_LIST, "True if the dependencies should be shown as a flat list.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_CREATE_BDIO, "True if BDIO files should be generated in the output directory.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_DEPLOY_BDIO, "True if the BDIO files in the output directory should be uploaded to the hub.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_CREATE_REPORT, "True if the risk report should be generated in the output directory.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_CHECK_POLICIES, "Check if the project contains policy violations in the Hub.");
            AddMenuOption(optionSet, PARAM_KEY_HUB_CODE_LOCATION_NAME, "Override for the code location to insert into the BDIO file.");

            optionSet.Add("?|h|help", "Display the information on how to use this executable.", value => ShowHelp = value != null);
            optionSet.Add("v|verbose", "Display more messages when the executable runs.", value => Verbose = value != null);

            // add help otion
            return optionSet;
        }

        private void AddMenuOption(OptionSet optionSet, string name, string description)
        {
            optionSet.Add($"{name}=", description, value => PropertyMap[name] = value );
        }
        
        private void ParseCommandLine(OptionSet commandOptions)
        {
            try
            {
                commandOptions.Parse(this.Args);
            }
            catch(OptionException)
            {
                ShowHelpMessage("Error processing command line, usage is: ", commandOptions);
            }
        }

        private void ConfigureGenerator()
        {
            if(!PropertyMap.ContainsKey(PARAM_KEY_SOLUTION))
            {
               throw new BlackDuckIntegrationException(String.Format("Missing required parameter {0}", PARAM_KEY_SOLUTION));
            }

            SolutionGenerator.Verbose = Verbose;
            SolutionGenerator.SolutionPath = GetPropertyValue(PARAM_KEY_SOLUTION);
            SolutionGenerator.HubUrl = GetPropertyValue(PARAM_KEY_HUB_URL);
            SolutionGenerator.HubUsername = GetPropertyValue(PARAM_KEY_HUB_USERNAME);
            SolutionGenerator.HubPassword = GetPropertyValue(PARAM_KEY_HUB_PASSWORD);
            SolutionGenerator.PackagesRepoUrl = GetPropertyValue(PARAM_KEY_PACKAGE_REPO_URL);

            SolutionGenerator.HubTimeout = Convert.ToInt32(GetPropertyValue(PARAM_KEY_HUB_TIMEOUT,"120"));
            SolutionGenerator.HubProjectName = GetPropertyValue(PARAM_KEY_HUB_PROJECT_NAME);
            SolutionGenerator.HubVersionName = GetPropertyValue(PARAM_KEY_HUB_VERSION_NAME);
           
            SolutionGenerator.HubProxyHost = GetPropertyValue(PARAM_KEY_HUB_PROXY_HOST);
            SolutionGenerator.HubProxyPort = GetPropertyValue(PARAM_KEY_HUB_PROXY_PORT);
            SolutionGenerator.HubProxyUsername = GetPropertyValue(PARAM_KEY_HUB_PROXY_USERNAME);
            SolutionGenerator.HubProxyPassword = GetPropertyValue(PARAM_KEY_HUB_PROXY_PASSWORD);

            SolutionGenerator.HubScanTimeout = Convert.ToInt32(GetPropertyValue(PARAM_KEY_HUB_SCAN_TIMEOUT,"300"));
            SolutionGenerator.OutputDirectory = GetPropertyValue(PARAM_KEY_HUB_OUTPUT_DIRECTORY);
            SolutionGenerator.ExcludedModules = GetPropertyValue(PARAM_KEY_EXCLUDED_MODULES);
            SolutionGenerator.HubIgnoreFailure = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_IGNORE_FAILURE,"false"));
            SolutionGenerator.CreateFlatDependencyList = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CREATE_FLAT_LIST,"false"));
            SolutionGenerator.CreateHubBdio = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CREATE_BDIO,"true"));
            SolutionGenerator.DeployHubBdio = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_DEPLOY_BDIO,"true"));
            SolutionGenerator.CreateHubReport = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CREATE_REPORT,"false"));
            SolutionGenerator.CheckPolicies = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CHECK_POLICIES, "false"));
            SolutionGenerator.HubCodeLocationName = GetPropertyValue(PARAM_KEY_HUB_CODE_LOCATION_NAME);
        }

        private string GetPropertyValue(string key, string defaultValue = "")
        {
            if(PropertyMap.ContainsKey(key))
            {
                return PropertyMap[key];
            }
            else
            {
                return defaultValue;
            }
        }
        
        private void ShowHelpMessage(string message, OptionSet optionSet)
        {
            Console.Error.WriteLine(message);
            optionSet.WriteOptionDescriptions(Console.Error);
            Environment.Exit(-1);
        }              
    }
}
