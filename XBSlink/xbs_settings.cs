/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_settings.cs
 *   
 * @author Oliver Seuffert, Copyright (C) 2011.
 */
/* 
 * XBSlink is free software; you can redistribute it and/or modify 
 * it under the terms of the GNU General Public License as published by 
 * the Free Software Foundation; either version 2 of the License, or 
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
 * for more details.
 * 
 * You should have received a copy of the GNU General Public License along 
 * with this program; If not, see <http://www.gnu.org/licenses/>
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.Win32;
using XBSlink.Properties;
using System.Configuration;

namespace XBSlink
{
    class xbs_settings
    {
        // Program version
        public static String xbslink_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public const int PROGRAM_UPDATE_CHECK_HOURS_INTERVAL = 12;

        // Registry
        public const String REG_CAPTURE_DEVICE_NAME = "capture device";
        public const String REG_LOCAL_IP = "local ip";
        public const String REG_LOCAL_PORT = "local port";
        public const String REG_REMOTE_HOST = "remote ip";
        public const String REG_REMOTE_HOST_HISTORY = "remote ip history";
        public const String REG_REMOTE_PORT = "remote port";
        public const String REG_USE_UPNP = "use UPnP";
        public const String REG_ADVANCED_BROADCAST_FORWARDING = "advanced broadcast forwarding";
        public const String REG_ENABLE_SPECIAL_MAC_LIST = "enable special mac list";
        public const String REG_ONLY_FORWARD_SPECIAL_MACS = "only forward special macs";
        public const String REG_SPECIAL_MAC_LIST = "special mac list";
        public const String REG_ENABLE_STUN_SERVER = "enable STUN server";
        public const String REG_STUN_SERVER_HOSTNAME = "STUN server hostname";
        public const String REG_STUN_SERVER_PORT = "STUN server port";
        public const String REG_CHAT_NICKNAME = "chat nickname";
        public const String REG_CHAT_AUTOSWITCH = "chat autoswitch";
        public const String REG_CHAT_SOUND_NOTIFICATION = "chat sound notification";
        public const String REG_NEW_NODE_SOUND_NOTIFICATION = "new node sound notification";
        public const String REG_CLOUDLIST_SERVER = "cloudlist server";
        public const String REG_USE_CLOUDLIST_SERVER_TO_CHECK_INCOMING_PORT = "use cloudlist server to check incoming port";
        public const String REG_CHECK4UPDATES = "check for updates";
        public const String REG_NAT_ENABLE = "enable NAT";
        public const String REG_NAT_LOCAL_BROADCAST = "local Broadcast";
        public const String REG_NAT_IP_POOL = "NAT IP pool";

        private static Dictionary<String, String> registry_settings = new Dictionary<String, String>();
        private static Dictionary<String, byte[]> registry_settings_binary = new Dictionary<String, byte[]>();
        private static RegistryKey regkey;

        public static Settings settings;
        private static Dictionary<String, String> properties = new Dictionary<String, String>();

        public xbs_settings()
        {
            settings = new Settings();
            initRegArray();            
        }
        public void initRegArray()
        {
            foreach (FieldInfo fi in typeof(xbs_settings).GetFields())
                if (fi.Name.StartsWith("REG_"))
                    xbs_settings.properties[(String)fi.GetValue(this)] = fi.Name;
        }

        public static void saveSettings()
        {
            settings.Save();
        }


        public static String getOnlineProgramVersion()
        {
            WebClient updatecheck_webclient = new WebClient();
            String url = Resources.url_check_latest_version;
            if (System.Environment.OSVersion.Platform == PlatformID.MacOSX)
                url = Resources.url_check_latest_version_mac;
            else if (System.Environment.OSVersion.Platform == PlatformID.Unix)
                url = Resources.url_check_latest_version_linux;

            Uri uri = new Uri(url+"?version="+xbs_settings.xbslink_version);
            String result;
            updatecheck_webclient.Proxy = null;
            try
            {
                result = updatecheck_webclient.DownloadString(uri);
            }
            catch (WebException wex)
            {
                xbs_messages.addInfoMessage("!! could not get online update version information: " + wex.Message);
                return null;
            }

            if (result.Length != 7)
            {
                xbs_messages.addInfoMessage("!! update server returned unknown result: " + result);
                return null;
            }
            else
                return result;
        }

    }
}
