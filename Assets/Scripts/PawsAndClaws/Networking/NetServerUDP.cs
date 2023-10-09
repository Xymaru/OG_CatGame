using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetServerUDP : NetServer
    {
        private Thread _updateThread;
        private EndPoint _endPoint = new IPEndPoint(IPAddress.Any, NetworkData.Port);
        public void Start()
        {
            _updateThread = new Thread(UpdateThread);
            _updateThread.Start();
        }

        void UpdateThread()
        {
            while (true)
            {
                int revSize = ReceivePacket(_serverSocket);
                Debug.Log($"Packet received with size {revSize}");
            }
        }

        private void OnDestroy()
        {
            if (_updateThread.IsAlive)
            {
                _updateThread.Abort();
                _serverSocket.Socket.Shutdown(SocketShutdown.Both);
                _serverSocket.Socket.Close();
            }
        }

        protected override int ReceivePacket(NetworkSocket socket)
        {
            OnPacketReceived?.Invoke();
            return socket.Socket.ReceiveFrom(PacketBytes, ref _endPoint);
        }

        protected override int SendPacket(object packet, NetworkSocket socket)
        {
            OnPacketSend?.Invoke();
            PacketBytes = Utils.BinaryUtils.ObjectToByteArray(packet);
            Debug.Log($"Server send packet to IP: {socket.IPAddr}");
            return socket.Socket.SendTo(PacketBytes, _endPoint);
        }
    }
}