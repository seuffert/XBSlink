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


        public xbs_settings()
        {
            regkey = Registry.CurrentUser.CreateSubKey(@"Software\XBSlink");
            loadRegistryValues();
        }

        private void loadRegistryValues()
        {
            lock (registry_settings)
            {
                foreach (String val_name in regkey.GetValueNames())
                {
                    RegistryValueKind value_kind = regkey.GetValueKind(val_name);
                    if (value_kind==RegistryValueKind.String)
                        registry_settings.Add(val_name, (String)regkey.GetValue(val_name));
                    else if ( value_kind == RegistryValueKind.Binary )
                        registry_settings_binary.Add(val_name, (byte[])regkey.GetValue(val_name));
                }
            }
        }

        public static void saveRegistryValues()
        {
            lock (registry_settings)
            {
                foreach (KeyValuePair<string, string> kvp in registry_settings)
                    regkey.SetValue(kvp.Key, kvp.Value == null ? "" : kvp.Value);
                foreach (KeyValuePair<string, byte[]> kvp in registry_settings_binary)
                    if (kvp.Value != null)
                        regkey.SetValue(kvp.Key, kvp.Value);
            }
        }

        public static String getRegistryValue(String value_name)
        {
            String ret=null;
            lock (registry_settings)
                if (registry_settings.ContainsKey(value_name))
                    ret = registry_settings[value_name];
            return ret;
        }
        public static bool getRegistryValue(String value_name, bool default_value)
        {
            bool ret = default_value;
            lock (registry_settings)
            {
                if (registry_settings.ContainsKey(value_name))
                    if (!Boolean.TryParse(registry_settings[value_name], out ret))
                        ret = default_value;
            }
            return ret;
        }
        public static byte[] getRegistryBinaryValue(String value_name)
        {
            lock (registry_settings)
                if (registry_settings_binary.ContainsKey(value_name))
                    return registry_settings_binary[value_name];
            return null;
        }


        public static void setRegistryValue(String value_name, String value)
        {
            lock (registry_settings)
                registry_settings[value_name] = value;
        }
        public static void setRegistryValue(String value_name, Object value)
        {
			if (value!=null)
                lock (registry_settings)
            	    setRegistryValue( value_name, value.ToString());
        }
        public static void setRegistryValue(String value_name, byte[] value)
        {
            if (value != null)
                lock (registry_settings)
                    registry_settings_binary[value_name] = value;
        }

        public static void initializeRegistrySettingWithControl(String value_name, CheckBox checkbox)
        {
            bool check = checkbox.Checked;
            lock (registry_settings)
                if ( xbs_settings.getRegistryValue(value_name) != null )
                    check = xbs_settings.getRegistryValue(value_name, check);
            checkbox.Checked = check;
        }
        public static void initializeRegistrySettingWithControl(String value_name, TextBox textbox)
        {
            String text = null;
            lock (registry_settings)
                if (xbs_settings.getRegistryValue(value_name) != null)
                    text = xbs_settings.getRegistryValue(value_name);
            if (text!=null)
                textbox.Text = text;
        }
        public static void initializeRegistrySettingWithControl(String value_name, ComboBox combobox)
        {
            String text = null;
            lock (registry_settings) 
                if (xbs_settings.getRegistryValue(value_name) != null)
                    text = xbs_settings.getRegistryValue(value_name);
            if (text != null)
            {
                if (!combobox.Items.Contains(text))
                    combobox.Items.Add(text);
                combobox.SelectedItem = text;
            }
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
