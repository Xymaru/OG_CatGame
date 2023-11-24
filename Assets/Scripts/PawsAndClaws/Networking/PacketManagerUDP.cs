using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PawsAndClaws.Game;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    class PacketStateUDP
    {
        public Socket socket;
        public EndPoint remoteEP;
        public EndPoint remoteInternetEP;
        public byte[] buffer;
    }

    public class PacketManagerUDP
    {

        public Action<NetworkPacket> OnPacketReceived;
        private Thread _thread;
        PacketStateUDP pstate;
        object mutex = new();
        public void BeginReceive(Socket socket)
        {
            pstate = new PacketStateUDP();
            pstate.socket = socket;
            pstate.buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                pstate.remoteEP = new IPEndPoint(NetworkData.NetSocket.IPAddr, NetworkData.PortUDP);
            }

            if (NetworkData.NetSocket.NetCon == NetCon.Host)
            {
                pstate.remoteEP = new IPEndPoint(IPAddress.Any, NetworkData.PortUDP);
                pstate.remoteInternetEP = new IPEndPoint(IPAddress.Any, 0);
                pstate.socket.Bind(pstate.remoteEP);
            }

            _thread = new Thread(() => ReceiveCB(pstate));
            _thread.Start();
        }

        public void SendPacket(byte[] data)
        {
            pstate.socket.SendTo(data, 0, NetworkPacket.MAX_BUFFER_SIZE, SocketFlags.None, pstate.remoteEP);
        }
        public void SendPacket(byte[] data, EndPoint endPoint)
        {
            pstate.socket.SendTo(data, 0, NetworkPacket.MAX_BUFFER_SIZE, SocketFlags.None, endPoint);
        }

        void ReceiveCB(PacketStateUDP pstate)
        {
            while (true)
            {
                try
                {
                    if (NetworkData.NetSocket.NetCon == NetCon.Client)
                    {
                        int bytes_read = pstate.socket.ReceiveFrom(pstate.buffer, SocketFlags.None, ref pstate.remoteEP);
                        if (bytes_read == 0)
                            return;

                        NetworkPacket npacket = NetworkPacket.FromByteArray(pstate.buffer);
                        OnPacketReceived?.Invoke(npacket);
                    }

                    if (NetworkData.NetSocket.NetCon == NetCon.Host)
                    {
                        int bytes_read = pstate.socket.ReceiveFrom(pstate.buffer, SocketFlags.None, ref pstate.remoteInternetEP);
                        if (bytes_read == 0)
                            return;
                        NetworkPacket npacket = NetworkPacket.FromByteArray(pstate.buffer);

                        OnPacketReceived?.Invoke(npacket);
                        pstate.remoteEP = new IPEndPoint(IPAddress.Any, NetworkData.PortUDP);
                    }
                }
                catch(Exception e) 
                {
                    Debug.LogError(e);
                }
                // Continue listening
                //pstate.socket.BeginReceiveFrom(pstate.buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, 0, ref pstate.remoteEP, new AsyncCallback(ReceiveCB), pstate);
            }
        }
        public void Close()
        {
            if (_thread.IsAlive)
            {
                _thread.Abort();
            }
        }
    }
}