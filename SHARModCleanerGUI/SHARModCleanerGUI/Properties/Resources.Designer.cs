﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SHARModCleanerGUI.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SHARModCleanerGUI.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to if not P3D then
        ///	dofile(GetModPath() .. &quot;/Resources/lib/P3D2.lua&quot;)
        ///end
        ///local Path = GetPath()
        ///local GamePath = &quot;/GameData/&quot; .. Path
        ///local DiffPath = GetModPath() .. &quot;/Resources/P3D_Diffs/&quot; .. Path
        ///local DiffLuaPath = DiffPath .. &quot;.lua&quot;
        ///
        ///if not Exists(GamePath, true, false) then
        ///	--print(&quot;P3D_Diffs&quot;, &quot;Could not find existing file: &quot; .. GamePath)
        ///	return
        ///end
        ///
        ///if not Exists(DiffPath, true, false) then
        ///	--print(&quot;P3D_Diffs&quot;, &quot;Could not find diff file: &quot; .. DiffPath)
        ///	return
        ///end
        ///
        ///if not Exists(D [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Handle_P3D_Diffs {
            get {
                return ResourceManager.GetString("Handle_P3D_Diffs", resourceCulture);
            }
        }
    }
}