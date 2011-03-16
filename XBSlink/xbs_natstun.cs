/**
 * Project: XBSlink: A XBox360 & PS3/2 System Link Proxy
 * File name: xbs_natstun.cs
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
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using stunlib;
using stunlib.Client;
using Mono.Nat;
using Mono.Nat.Pmp;
using Mono.Nat.Upnp;

namespace XBSlink
{
    class xbs_natstun
    {
        // NAT variables
        private INatDevice device = null;
        private IPAddress public_ip = null;
        private List<Mapping> my_PortMappings = new List<Mapping>();

        // STUN variables
        public const int STUN_SERVER_DEFAULT_PORT = 3478;
        public const String STUN_SERVER_DEFAULT_HOSTNAME = "stunserver.org";
        private String stun_server_hostname = "stunserver.org";
        private int stun_server_port = 3478;
        private STUN_Result stun_result = null;
        private Thread discoverStunType_thread = null;
        private bool stun_server_discovery_done = false;

        public static bool isPortReachable = false;

        // External IP Service
        private const String EXTERNAL_IP_WEB_SERVICE = "http://www.whatismyip.com/automation/n09230945.asp";

        public xbs_natstun()
        {
            NatUtility.DeviceFound += upnp_device_found;
            NatUtility.UnhandledException += upnp_unhandled_exception;
        }

        #region --------- NAT stuff ---------

        public void upnp_startDiscovery()
        {
            if (!isUPnPavailable())
            {
                xbs_messages.addInfoMessage(" @ UPnP device discovery started");
                NatUtility.StartDiscovery();
            }
        }

        private void upnp_device_found(object sender, DeviceEventArgs args)
        {
            INatDevice dev = args.Device;
            IPAddress pub_ip = null;
            try
            {
                pub_ip = dev.GetExternalIP();
            }
            catch (Exception)
            {
                device = null;
                public_ip = null;
            }
            if (dev!=null && pub_ip!=null)
            {
                lock (this)
                {
                    this.device = dev;
                    this.public_ip = pub_ip;
                }
                xbs_messages.addInfoMessage(" @ UPnP device found. external IP: " + pub_ip);
            }
            else
                xbs_messages.addInfoMessage(" @ UPnP discovery failed. Could not get public IP");
        }

        public bool upnp_create_mapping(Protocol prot, int internalPort, int externalPort)
        {
            if (isUPnPavailable())
            {
                Mapping port_mapping = new Mapping(prot, internalPort, externalPort);
                try
                {
                    device.CreatePortMap(port_mapping);
                }
                catch (MappingException)
                {
                    xbs_messages.addInfoMessage(" @ UPnP error: could not forward port");
                    return false;
                }
                lock (this)
                    my_PortMappings.Add(port_mapping);
                xbs_messages.addInfoMessage(" @ UPnP port mapped from " + public_ip+":"+port_mapping.PublicPort);
                return true;
            }
            return false;
        }

        public void upnp_deleteAllPortMappings()
        {
            if (isUPnPavailable())
            {
                List<Mapping> mappings;
                lock (this)
                {
                    mappings = this.my_PortMappings;
                    this.my_PortMappings = new List<Mapping>();
                }
                try
                {
                    foreach (Mapping pm in mappings)
                        device.DeletePortMap(pm);
                }
                catch (Exception)
                {
                }
            }
        }

        private void upnp_unhandled_exception(object sender, UnhandledExceptionEventArgs args)
        {
            xbs_messages.addInfoMessage(" @ UPnP error: " + args.ExceptionObject.ToString());
        }

        public IPAddress upnp_getPublicIP()
        {
            return public_ip;
        }

        public bool isUPnPavailable()
        {
            bool ret = false;
            lock (this)
                ret = (this.device != null);
            return ret;
        }
        #endregion

        #region --------- STUN stuff ---------
        public void stun_startDiscoverStunType( String hostname, int port )
        {
            this.stun_server_hostname = hostname;
            this.stun_server_port = port;
            xbs_messages.addInfoMessage(" @ STUN server discovery at " + stun_server_hostname +":"+stun_server_port);
            discoverStunType_thread = new Thread(new ThreadStart(stun_asyncDiscoverStunType));
            discoverStunType_thread.IsBackground = true;
            discoverStunType_thread.Start();
        }

        private void stun_asyncDiscoverStunType()
        {
            STUN_Result result = null;
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Bind(new IPEndPoint(IPAddress.Any, 0));
                result = STUN_Client.Query( stun_server_hostname, stun_server_port, socket);
            }
            catch (Exception ex)
            {
                xbs_messages.addInfoMessage(" @ STUN server error: " + ex.Message);
            }

            lock (this)
            {
                stun_result = result;
                stun_server_discovery_done = true;
            }
            if (result != null)
            {
                String public_ip = result.PublicEndPoint != null ? result.PublicEndPoint.Address.ToString() : "(N/A)";
                xbs_messages.addInfoMessage(" @ STUN result: " + result.NetType.ToString() + " on " + public_ip);
            }
        }

        public bool stun_isServerDiscoverySuccessfull()
        {
            bool ret;
            lock (this)
                ret = stun_server_discovery_done && (stun_result != null);
            return ret;
        }

        public bool stun_isDiscoveryFinished()
        {
            bool ret;
            lock (this)
                ret = stun_server_discovery_done;
            return ret;
        }
        public STUN_Result stun_getResult()
        {
            STUN_Result result = null;
            lock (this)
                result = this.stun_result;
            return result;
        }
        #endregion

        public static IPAddress getExternalIPAddressFromWebsite()
        {
            String external_ip_str;
            IPAddress external_ip;
            try
            {
                external_ip_str = new System.Net.WebClient().DownloadString(xbs_natstun.EXTERNAL_IP_WEB_SERVICE);
            }
            catch (WebException)
            {
                xbs_messages.addInfoMessage("!! Could not resolve external IP Address.");
                return null;
            }
            bool ret = IPAddress.TryParse(external_ip_str.Trim(), out external_ip);
            if ( ret )
                xbs_messages.addInfoMessage(" @ discovered external public IP " + external_ip);
            return ( ret ? external_ip : null);
        }

    }
}
