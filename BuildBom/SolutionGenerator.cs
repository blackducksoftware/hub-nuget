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
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple;
using Com.Blackducksoftware.Integration.Hub.Bdio.Simple.Model;
using Com.Blackducksoftware.Integration.Hub.Common.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Com.Blackducksoftware.Integration.Hub.Nuget.BuildBom
{
    public class SolutionGenerator : ProjectGenerator
    {
        public string SolutionPath { get; set; }
        public bool GenerateMergedBdio { get; set; }

        public override bool Execute()
        {
            bool result = true;
            string originalOutputDirectory = OutputDirectory;
            string originalHubProjectName = HubProjectName;
            string originalHubVersionName = HubVersionName;

            List<string> alreadyMergedComponents = new List<string>();
            List<BdioNode> mergedComponentList = new List<BdioNode>();

            try
            {
                // TODO: clean up this code to generate the BDIO first then perform the deploy and checks for each project
                // Also aggregate the results of the check policies.
                Dictionary<string, string> projectData = ParseSolutionFile(SolutionPath);
                Console.WriteLine("Parsed Solution File");
                if (projectData.Count > 0)
                {
                    string solutionDirectory = Path.GetDirectoryName(SolutionPath);
                    Console.WriteLine("Solution directory: {0}", solutionDirectory);
                    foreach (string key in projectData.Keys)
                    {
                        if (String.IsNullOrWhiteSpace(originalOutputDirectory))
                        {
                            OutputDirectory = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}{key}";
                        }
                        else
                        {
                            OutputDirectory = $"{originalOutputDirectory}{Path.DirectorySeparatorChar}{key}";
                        }
                        string projectRelativePath = projectData[key];
                        List<string> projectPathSegments = new List<string>();
                        projectPathSegments.Add(solutionDirectory);
                        projectPathSegments.Add(projectRelativePath);

                        ProjectPath = CreatePath(projectPathSegments);

                        if (String.IsNullOrWhiteSpace(originalHubProjectName))
                        {
                            HubProjectName = key;
                        }

                        if (String.IsNullOrWhiteSpace(originalHubVersionName))
                        {
                            HubVersionName = originalHubVersionName;
                        }

                        bool projectResult = base.Execute();
                        PackagesConfigPath = ""; // reset to use the project packages file.
                        result = result && projectResult;

                        if (projectResult && GenerateMergedBdio)
                        {
                            string bdioFilePath = $"{OutputDirectory}{Path.DirectorySeparatorChar}{HubProjectName}.jsonld";
                            string bdio = File.ReadAllText(bdioFilePath);
                            BdioContent bdioContent = BdioContent.Parse(bdio);

                            foreach (BdioComponent component in bdioContent.Components)
                            {
                                if (!alreadyMergedComponents.Contains(component.BdioExternalIdentifier.ExternalId))
                                {
                                    mergedComponentList.Add(component);
                                    alreadyMergedComponents.Add(component.BdioExternalIdentifier.ExternalId);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No project data found for solution {0}", SolutionPath);
                }
            }
            catch (Exception ex)
            {
                if (HubIgnoreFailure)
                {
                    result = true;
                    Console.WriteLine("Error executing Build BOM task. Cause: {0}", ex);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                OutputDirectory = originalOutputDirectory; // reset the output directory to original path
            }

            //Generate after so the "output directory" is the one for the solution, not just the last project processed
            if (GenerateMergedBdio)
            {
                GenerateMergedFile(mergedComponentList);
            }

            return result;
        }
        
        private Dictionary<string, string> ParseSolutionFile(string solutionPath)
        {
            Dictionary<string, string> projectDataMap = new Dictionary<string, string>();
            // Visual Studio right now is not resolving the Microsoft.Build.Construction.SolutionFile type
            // parsing the solution file manually for now.
            if (File.Exists(solutionPath))
            {
                List<string> contents = new List<string>(File.ReadAllLines(solutionPath));
                var projectLines = contents.FindAll(text => text.StartsWith("Project("));
                foreach (string projectText in projectLines)
                {
                    int equalIndex = projectText.IndexOf("=");
                    if (equalIndex > -1)
                    {
                        string projectValuesCSV = projectText.Substring(equalIndex + 1);
                        projectValuesCSV = projectValuesCSV.Replace("\"", "");
                        string[] projectValues = projectValuesCSV.Split(new char[] { ',' });

                        if (projectValues.Length >= 2)
                        {
                            projectDataMap[projectValues[0].Trim()] = projectValues[1].Trim();
                        }
                    }
                }
                Console.WriteLine("Black Duck I/O Generation Found {0} Project elements, processed {1} project elements for data", projectLines.Count(), projectDataMap.Count());
            }
            else
            {
                throw new BlackDuckIntegrationException("Solution File " + solutionPath + " not found");
            }

            return projectDataMap;
        }

        private void GenerateMergedFile(List<BdioNode> components)
        {
            BdioPropertyHelper bdioPropertyHelper = new BdioPropertyHelper();
            BdioNodeFactory bdioNodeFactory = new BdioNodeFactory(bdioPropertyHelper);
            BdioContent bdio = new BdioContent();

            // Create bdio bill of materials node
            BdioBillOfMaterials bdioBillOfMaterials = bdioNodeFactory.CreateBillOfMaterials(HubCodeLocationName, HubProjectName, HubVersionName);

            // Create bdio project node
            string projectBdioId = bdioPropertyHelper.CreateBdioId(HubProjectName, HubVersionName);
            BdioExternalIdentifier projectExternalIdentifier = bdioPropertyHelper.CreateNugetExternalIdentifier(HubProjectName, HubVersionName); // Note: Could be different. Look at config file
            BdioProject bdioProject = bdioNodeFactory.CreateProject(HubProjectName, HubVersionName, projectBdioId, projectExternalIdentifier);

            bdio.BillOfMaterials = bdioBillOfMaterials;
            bdio.Project = bdioProject;
            bdio.Components = components;

            string bdioFilePath = $"{OutputDirectory}{Path.DirectorySeparatorChar}{HubProjectName}.jsonld";
            File.WriteAllText(bdioFilePath, bdio.ToString());
        }
    }
}
