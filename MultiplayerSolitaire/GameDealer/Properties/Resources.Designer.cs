﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameDealer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GameDealer.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to function requestNewGame() {
        ///    var xhttp = new XMLHttpRequest();
        ///    xhttp.onreadystatechange = function () {
        ///        if (this.readyState == 4 &amp;&amp; this.status == 200) {
        ///            document.getElementsByClassName(&quot;Response&quot;).innerHTML = this.responseText;
        ///        }
        ///    };
        ///    xhttp.open(&quot;POST&quot;, &quot;RequestNewGame&quot;, true);
        ///    xhttp.send();
        ///}.
        /// </summary>
        internal static string GameDealer {
            get {
                return ResourceManager.GetString("GameDealer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
        ///&lt;html&gt;
        ///    &lt;head&gt;
        ///        &lt;meta http-equiv=&quot;Content-Type&quot; content=&quot;text/html; charset=utf-8&quot;&gt;
        ///        &lt;title&gt;Game Dealer&lt;/title&gt;
        ///        &lt;link rel=&quot;stylesheet&quot; type=&quot;text/css&quot; href=&quot;style.css&quot; /&gt;
        ///    &lt;/head&gt;
        ///    &lt;body&gt;
        ///        &lt;div class=&quot;Title&quot;&gt;Two Players Solitaire&lt;/div&gt;
        ///        &lt;button onclick=&quot;requestNewGame()&quot;&gt;New Game&lt;/button&gt;
        ///        &lt;div class=&quot;Response&quot;&gt;&lt;/div&gt;
        ///        &lt;div id=&quot;signature&quot;&gt;
        ///            Made by &lt;a href=&quot;https://antonmakesgames.itch.io/&quot; target=&quot;_blank&quot; rel= [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string index {
            get {
                return ResourceManager.GetString("index", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to body {
        ///    font-family: &quot;Times New Roman&quot;, Times, serif;
        ///}
        ///.
        /// </summary>
        internal static string style {
            get {
                return ResourceManager.GetString("style", resourceCulture);
            }
        }
    }
}
