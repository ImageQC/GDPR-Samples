﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gdpr.DomainTests.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Gdpr.DomainTests.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Database command failed.  Error in SQL statement, parameters or table. Please report this problem..
        /// </summary>
        internal static string MxErrDbCmdException {
            get {
                return ResourceManager.GetString("MxErrDbCmdException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot contact the database. Connection cannot be opened at this time. Please try again later.
        /// </summary>
        internal static string MxErrDbConnClosed {
            get {
                return ResourceManager.GetString("MxErrDbConnClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Data connection error. Connection details may be incorrect. Please report this problem.
        /// </summary>
        internal static string MxErrDbConnException {
            get {
                return ResourceManager.GetString("MxErrDbConnException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database connection invalid. Connection string possibily not set. Please report this problem.
        /// </summary>
        internal static string MxErrDbConnNotSet {
            get {
                return ResourceManager.GetString("MxErrDbConnNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Database query failed.  Error in SQL statement, parameters or table. Please report this problem.
        /// </summary>
        internal static string MxErrDbQueryException {
            get {
                return ResourceManager.GetString("MxErrDbQueryException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error test. Coding defect. Please report this problem.
        /// </summary>
        internal static string MxErrTest {
            get {
                return ResourceManager.GetString("MxErrTest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error information not found. Coding defect. Please report this problem.
        /// </summary>
        internal static string MxMsgNotFound {
            get {
                return ResourceManager.GetString("MxMsgNotFound", resourceCulture);
            }
        }
    }
}
