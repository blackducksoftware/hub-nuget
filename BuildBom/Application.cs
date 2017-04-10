/*******************************************************************************
 * Copyright (C) 2017 Black Duck Software, Inc.
 * http://www.blackducksoftware.com/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 *******************************************************************************/
using Mono.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.BuildBom
{
    public class Application
    {
        public const string PARAM_KEY_APP_SETTINGS_FILE = "app_settings_file";
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

        private ProjectGenerator ProjectGenerator;
        private string[] Args;
        private Dictionary<string, string> PropertyMap;
        private Dictionary<string, string> CommandLinePropertyMap;
        private Dictionary<string, string> AppSettingsMap;

        private bool ShowHelp = false;
        private bool Verbose = false;

        public Application(string [] args)
        {
            this.Args = args;
            PropertyMap = new Dictionary<string, string>();
            CommandLinePropertyMap = new Dictionary<string, string>();
            AppSettingsMap = new Dictionary<string, string>();
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
        
        public bool Execute()
        {
            try
            {
                Configure();
                if (!ShowHelp)
                {
                    return ProjectGenerator.Execute();
                }
                else
                {
                    return false;
                }
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
            string usageMessage = "Usage is BuildBom.exe [OPTIONS]";
            
            if(ShowHelp)
            {
                LogProperties();
                ShowHelpMessage(usageMessage, commandOptions);
            }

            ConfigureGenerator(commandOptions);
        }

        private void LogProperties()
        {
            if (Verbose)
            {
                Console.WriteLine("Configuration Properties: ");
                foreach (string key in PropertyMap.Keys)
                {
                    string property_value = PropertyMap[key];
                    if (key.Contains("password"))
                    {
                        Console.WriteLine("Property {0} = **********", key);
                    }
                    else
                    {
                        Console.WriteLine("Property {0} = {1}", key, PropertyMap[key]);
                    }
                }
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
            AddAppSettingsFileMenuOption(optionSet, PARAM_KEY_APP_SETTINGS_FILE, "The file path for the application settings that overrides all settings.");
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

        private void AddAppSettingsFileMenuOption(OptionSet optionSet, string name, string description)
        {
            optionSet.Add($"{name}=", description, (value) =>
            {
                string appSettingsFile = value;
                if (!string.IsNullOrWhiteSpace(appSettingsFile))
                {
                    PopulatePropertyMapByExternalFile(appSettingsFile);
                }
            });            
        }

        private void AddMenuOption(OptionSet optionSet, string name, string description)
        {
            optionSet.Add($"{name}=", description, (value) => 
            {
                CommandLinePropertyMap[name] = value;
            });
        }
        
        private void ParseCommandLine(OptionSet commandOptions)
        {
            try
            {
                commandOptions.Parse(this.Args);
            }
            catch(OptionException)
            {
                ShowHelpMessage("Error processing command line, usage is: buildBom.exe [OPTIONS]", commandOptions);
            }
        }

        private void PopulatePropertyMapByExternalFile(string path)
        {            
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = path;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap,ConfigurationUserLevel.None);
            foreach(KeyValueConfigurationElement element in config.AppSettings.Settings)
            {
                AppSettingsMap[element.Key] = element.Value;
            }
        }

        private void ResolveProperties()
        {
            foreach (string key in AppSettingsMap.Keys)
            {
                PropertyMap[key] = AppSettingsMap[key];
            }

            foreach (string key in CommandLinePropertyMap.Keys)
            {
                PropertyMap[key] = CommandLinePropertyMap[key];
            }
        }

        private void ConfigureGenerator(OptionSet commandOptions)
        {
            ResolveProperties();
            ProjectGenerator = CreateGenerator();

            if(ProjectGenerator == null)
            {
                ShowHelpMessage("Couldn't find a solution or project. Usage buildBom.exe [OPTIONS]", commandOptions);
            }

            if(PropertyMap.ContainsKey(PARAM_KEY_HUB_OUTPUT_DIRECTORY))
            {
                if (String.IsNullOrWhiteSpace(PropertyMap[PARAM_KEY_HUB_OUTPUT_DIRECTORY]))
                {
                    string currentDirectory = Directory.GetCurrentDirectory();
                    string defaultOutputDirectory = $"{currentDirectory}{Path.DirectorySeparatorChar}{ProjectGenerator.DEFAULT_OUTPUT_DIRECTORY}";
                    PropertyMap[PARAM_KEY_HUB_OUTPUT_DIRECTORY] = defaultOutputDirectory;
                }
            }

            LogProperties();

            ProjectGenerator.Verbose = Verbose;
            ProjectGenerator.HubUrl = GetPropertyValue(PARAM_KEY_HUB_URL);
            ProjectGenerator.HubUsername = GetPropertyValue(PARAM_KEY_HUB_USERNAME);
            ProjectGenerator.HubPassword = GetPropertyValue(PARAM_KEY_HUB_PASSWORD);
            ProjectGenerator.PackagesRepoUrl = GetPropertyValue(PARAM_KEY_PACKAGE_REPO_URL);

            ProjectGenerator.HubTimeout = Convert.ToInt32(GetPropertyValue(PARAM_KEY_HUB_TIMEOUT,"120"));
            ProjectGenerator.HubProjectName = GetPropertyValue(PARAM_KEY_HUB_PROJECT_NAME);
            ProjectGenerator.HubVersionName = GetPropertyValue(PARAM_KEY_HUB_VERSION_NAME);

            ProjectGenerator.HubProxyHost = GetPropertyValue(PARAM_KEY_HUB_PROXY_HOST);
            ProjectGenerator.HubProxyPort = GetPropertyValue(PARAM_KEY_HUB_PROXY_PORT);
            ProjectGenerator.HubProxyUsername = GetPropertyValue(PARAM_KEY_HUB_PROXY_USERNAME);
            ProjectGenerator.HubProxyPassword = GetPropertyValue(PARAM_KEY_HUB_PROXY_PASSWORD);

            ProjectGenerator.HubScanTimeout = Convert.ToInt32(GetPropertyValue(PARAM_KEY_HUB_SCAN_TIMEOUT,"300"));
            ProjectGenerator.OutputDirectory = GetPropertyValue(PARAM_KEY_HUB_OUTPUT_DIRECTORY);
            ProjectGenerator.ExcludedModules = GetPropertyValue(PARAM_KEY_EXCLUDED_MODULES);
            ProjectGenerator.HubIgnoreFailure = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_IGNORE_FAILURE,"false"));
            ProjectGenerator.CreateFlatDependencyList = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CREATE_FLAT_LIST,"false"));
            ProjectGenerator.CreateHubBdio = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CREATE_BDIO,"true"));
            ProjectGenerator.DeployHubBdio = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_DEPLOY_BDIO,"true"));
            ProjectGenerator.CreateHubReport = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CREATE_REPORT,"false"));
            ProjectGenerator.CheckPolicies = Convert.ToBoolean(GetPropertyValue(PARAM_KEY_HUB_CHECK_POLICIES, "false"));
            ProjectGenerator.HubCodeLocationName = GetPropertyValue(PARAM_KEY_HUB_CODE_LOCATION_NAME);
        }

        private ProjectGenerator CreateGenerator()
        {
            string solutionPath = GetPropertyValue(PARAM_KEY_SOLUTION);
            if (string.IsNullOrWhiteSpace(solutionPath))
            {
                Console.WriteLine("Searching for a solution file to process...");
                // search for solution
                string currentDirectory = Directory.GetCurrentDirectory();
                string[] solutionPaths = Directory.GetFiles(currentDirectory, "*.sln");

                if (solutionPaths != null && solutionPaths.Length >= 1)
                {
                    SolutionGenerator solutionGenerator = new SolutionGenerator();
                    solutionGenerator.SolutionPath = solutionPaths[0];
                    PropertyMap[PARAM_KEY_SOLUTION] = solutionPaths[0];
                    return solutionGenerator;
                }
                else
                {
                    Console.WriteLine("No Solution file found.  Searching for a project file...");
                    string[] projectPaths = Directory.GetFiles(currentDirectory, "*.csproj");
                    if (projectPaths != null && projectPaths.Length > 0)
                    {
                        string projectPath = projectPaths[0];
                        Console.WriteLine("Found project {0}", projectPath);
                        ProjectGenerator projectGenerator = new ProjectGenerator();
                        projectGenerator.ProjectPath = projectPath;
                        return projectGenerator;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                if (solutionPath.Contains(".sln"))
                {
                    SolutionGenerator solutionGenerator = new SolutionGenerator();
                    solutionGenerator.SolutionPath = solutionPath;

                    return solutionGenerator;
                }
                else
                {
                    ProjectGenerator projectGenerator = new ProjectGenerator();
                    projectGenerator.ProjectPath = solutionPath;
                    return projectGenerator;
                }
            }
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
