﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CefEOBrowser.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CefEOBrowser.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to function applyStyle(){{var e=document.getElementById(&quot;{0}&quot;);e&amp;&amp;document.head.removeChild(e),(e=document.createElement(&quot;style&quot;)).id=&quot;{0}&quot;,e.innerHTML=&quot;#flashWrap {{ position: fixed; left: 0; top: 0; width: 100%; height: 100%; }} #htmlWrap {{ width: 100% !important; height: 100% !important; }} #sectionWrap {{ display:none !important; }}&quot;,document.head.appendChild(e)}}&quot;loading&quot;===document.readyState?document.addEventListener(&quot;DOMContentLoaded&quot;,applyStyle):applyStyle();.
        /// </summary>
        internal static string Frame_JS {
            get {
                return ResourceManager.GetString("Frame_JS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to function applyStyle(){{var e=document.getElementById(&quot;{0}&quot;);e&amp;&amp;document.head.removeChild(e),(e=document.createElement(&quot;style&quot;)).id=&quot;{0}&quot;,e.innerHTML=&quot;body {{ visibility: hidden; overflow: hidden; }} div #block_background {{ visibility: visible; }} div #alert {{ visibility: visible; overflow: scroll; top: 0 !important; left: 3% !important; width: 90% !important; height: 100%; padding:2%;}} div.dmm-ntgnavi {{ display: none; }} #area-game {{ position: fixed; left: 0; top: 0; width: 100%; height: 100%; }} #game [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Page_JS {
            get {
                return ResourceManager.GetString("Page_JS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to !function(){{var e=document.getElementById(&apos;{0}&apos;);e&amp;&amp;document.head.removeChild(e)}}();.
        /// </summary>
        internal static string Restore_JS {
            get {
                return ResourceManager.GetString("Restore_JS", resourceCulture);
            }
        }
    }
}
