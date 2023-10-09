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
        private Thread _receiveThread;
        private EndPoint _endPoint = new IPEndPoint(IPAddress.Any, NetworkData.Port);
        public void Start()
        {
            _receiveThread = new Thread(ReceiveJob);
            _receiveThread.Start();
        }

        void ReceiveJob()
        {
            while (true)
            {
                int revSize = ReceivePacket(_serverSocket);
                Debug.Log($"Packet received with size {revSize}");
            }
        }

        private void OnDestroy()
        {
            if (_receiveThread.IsAlive)
            {
                _receiveThread.Abort();
                _serverSocket.Socket.Shutdown(SocketShutdown.Both);
                _serverSocket.Socket.Close();
            }
        }

        protected override int ReceivePacket(NetworkSocket socket)
        {
            OnPacketReceived?.Invoke();
            return socket.Socket.ReceiveFrom(PacketBytes, ref _endPoint);
        }

        public override int SendPacket(NetPacket packet, NetworkSocket socket)
        {
            OnPacketSend?.Invoke();
            PacketBytes = Utils.BinaryUtils.ObjectToByteArray(packet);
            Debug.Log($"Server send packet to IP: {socket.IPAddr}");
            return socket.Socket.SendTo(PacketBytes, _endPoint);
        }
    }
}