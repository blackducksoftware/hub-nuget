﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace com.blackducksoftware.integration.hub.nuget.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("com.blackducksoftware.integration.hub.nuget.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Test Name:	ExecuteTaskTest
        ///Test Outcome:	Passed
        ///Result StandardOutput:	
        ///[
        ///  {
        ///    &quot;specVersion&quot;: &quot;1.1.0&quot;,
        ///    &quot;@id&quot;: &quot;uuid:4f12abf6-f105-4546-b9c8-83c98a8611c5&quot;,
        ///    &quot;@type&quot;: &quot;BillOfMaterials&quot;,
        ///    &quot;name&quot;: &quot;BuildBomTask Black Duck I/O Export&quot;,
        ///    &quot;relationship&quot;: []
        ///  },
        ///  {
        ///    &quot;revision&quot;: &quot;0.0.1&quot;,
        ///    &quot;@id&quot;: &quot;data:BuildBomTask/0.0.1&quot;,
        ///    &quot;@type&quot;: &quot;Project&quot;,
        ///    &quot;name&quot;: &quot;BuildBomTask&quot;,
        ///    &quot;externalIdentifier&quot;: {
        ///      &quot;externalSystemTypeId&quot;: &quot;nuget&quot;,
        ///      &quot;externalId&quot;: &quot;BuildBomTask/0. [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string sample {
            get {
                return ResourceManager.GetString("sample", resourceCulture);
            }
        }
    }
}
