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

namespace XBSlink
{
    class xbs_settings
    {
        // Program version
        public static String xbslink_version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

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

        private static Dictionary<String, String> registry_settings = new Dictionary<String, String>();
        private static RegistryKey regkey;


        public xbs_settings()
        {
            regkey = Registry.CurrentUser.CreateSubKey(@"Software\XBSlink");
            loadRegistryValues();
        }

        private void loadRegistryValues()
        {
            foreach (String val_name in regkey.GetValueNames())
                registry_settings.Add(val_name, (String)regkey.GetValue(val_name));
        }

        public static void saveRegistryValues()
        {
            foreach (KeyValuePair<string, string> kvp in registry_settings)
            {
                regkey.SetValue(kvp.Key, kvp.Value == null ? "" : kvp.Value);
            }
        }

        public static String getRegistryValue(String value_name)
        {
            if (registry_settings.ContainsKey(value_name))
                return registry_settings[value_name];
            return null;
        }

        public static bool getRegistryValue(String value_name, bool default_value)
        {
            if (!registry_settings.ContainsKey(value_name))
                return default_value;
            bool c;
            if (Boolean.TryParse(registry_settings[value_name], out c))
                return c;
            return default_value;
        }

        public static void setRegistryValue(String value_name, String value)
        {
            registry_settings[value_name] = value;
        }

        public static void setRegistryValue(String value_name, Object value)
        {
			if (value!=null)
            	setRegistryValue( value_name, value.ToString());
        }

        public static void initializeRegistrySettingWithControl(String value_name, CheckBox checkbox)
        {
            if ( xbs_settings.getRegistryValue(value_name) != null )
                checkbox.Checked = xbs_settings.getRegistryValue(value_name, checkbox.Checked);
        }
        public static void initializeRegistrySettingWithControl(String value_name, TextBox textbox)
        {
            if (xbs_settings.getRegistryValue(value_name) != null)
                textbox.Text = xbs_settings.getRegistryValue(value_name);
        }
        public static void initializeRegistrySettingWithControl(String value_name, ComboBox combobox)
        {
            if ( xbs_settings.getRegistryValue(value_name) != null )
                combobox.SelectedItem = xbs_settings.getRegistryValue(value_name);
        }

    }
}
