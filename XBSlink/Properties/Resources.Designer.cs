﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XBSlink.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XBSlink.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to {\rtf1\adeflang1025\ansi\ansicpg1252\uc1\adeff0\deff0\stshfdbch31505\stshfloch31506\stshfhich31506\stshfbi0\deflang1031\deflangfe1031\themelang1031\themelangfe0\themelangcs0{\fonttbl{\f0\fbidi \froman\fcharset0\fprq2{\*\panose 02020603050405020304}Times New Roman;}{\f1\fbidi \fswiss\fcharset0\fprq2{\*\panose 020b0604020202020204}Arial;}
        ///{\f2\fbidi \fmodern\fcharset0\fprq1{\*\panose 02070309020205020404}Courier New;}{\f3\fbidi \froman\fcharset2\fprq2{\*\panose 05050102010706020507}Symbol;}{\f10\fbidi \fnil\ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string about_xbslink {
            get {
                return ResourceManager.GetString("about_xbslink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry, XBSlink crashed.
        ///Please send this info to xbslink@secudb.de, we might have the right bug spray ;) Thank you!.
        /// </summary>
        internal static string crash_message {
            get {
                return ResourceManager.GetString("crash_message", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap error_16 {
            get {
                object obj = ResourceManager.GetObject("error_16", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap error_50 {
            get {
                object obj = ResourceManager.GetObject("error_50", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap icon_key {
            get {
                object obj = ResourceManager.GetObject("icon_key", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XBSlink could not find any capture devices in your system.
        ///This problem occurs if the WinPCap library is not active. Please make sure WinPCap is loaded.
        ///
        ///For more information see &lt;http://www.secudb.de/~seuffert/xbslink/faq#question-15&gt;.
        /// </summary>
        internal static string message_no_capture_devices {
            get {
                return ResourceManager.GetString("message_no_capture_devices", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XBSlink could not find any capture devices in your system.
        ///This problem occurs if the WinPCap library is not active. Please make sure WinPCap is loaded.
        ///
        ///Do you want XBSlink to try starting the WinPcap driver (NPF) ?.
        /// </summary>
        internal static string message_no_capture_devices_startNPF {
            get {
                return ResourceManager.GetString("message_no_capture_devices_startNPF", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XBSlink could not find any capture devices in your system.
        ///This problem occurs if the user has not enough rights to use the pcap library. Please try to run XBSlink as root (&quot;sudo mono XBSlink.exe&quot;)
        ///
        ///For more information see &lt;http://www.secudb.de/~seuffert/xbslink/faq#question-2&gt;.
        /// </summary>
        internal static string message_no_capture_devices_unix {
            get {
                return ResourceManager.GetString("message_no_capture_devices_unix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please add at least one MAC address to the list..
        /// </summary>
        internal static string message_specialmaclist_empty {
            get {
                return ResourceManager.GetString("message_specialmaclist_empty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XBSlink encountered at least one non-fatal error!
        ///Please check the messages..
        /// </summary>
        internal static string notifyicon_error_message {
            get {
                return ResourceManager.GetString("notifyicon_error_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XBSlink encountered at least one fatal error! 
        ///Please check the messages..
        /// </summary>
        internal static string notifyicon_fatal_error_message {
            get {
                return ResourceManager.GetString("notifyicon_fatal_error_message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XBSlink encountered at least one warning!
        ///Please check the messages..
        /// </summary>
        internal static string notifyicon_warning_message {
            get {
                return ResourceManager.GetString("notifyicon_warning_message", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap ok_16 {
            get {
                object obj = ResourceManager.GetObject("ok_16", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.secudb.de/~seuffert/xbslink/latest_xbslink_version.
        /// </summary>
        internal static string url_check_latest_version {
            get {
                return ResourceManager.GetString("url_check_latest_version", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.secudb.de/~seuffert/xbslink/latest_xbslink_version_linux.
        /// </summary>
        internal static string url_check_latest_version_linux {
            get {
                return ResourceManager.GetString("url_check_latest_version_linux", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.secudb.de/~seuffert/xbslink/latest_xbslink_version_mac.
        /// </summary>
        internal static string url_check_latest_version_mac {
            get {
                return ResourceManager.GetString("url_check_latest_version_mac", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://www.secudb.de/~seuffert/xbslink/.
        /// </summary>
        internal static string url_xbslink_website {
            get {
                return ResourceManager.GetString("url_xbslink_website", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap warning_16 {
            get {
                object obj = ResourceManager.GetObject("warning_16", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Icon XBSlink {
            get {
                object obj = ResourceManager.GetObject("XBSlink", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
    }
}
