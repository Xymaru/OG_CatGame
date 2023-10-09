using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public abstract class NetServer : MonoBehaviour
    {
        protected NetworkServerSocket _serverSocket;
        protected NetworkManager _networkManager;
        public byte[] PacketBytes { get; protected set; } = new byte[2048];

        public static Action OnPacketReceived;
        public static Action OnPacketSend;

        private void Awake()
        {
            _serverSocket = (NetworkServerSocket)NetworkData.NetSocket;
            _networkManager = GetComponent<NetworkManager>();
        }

        protected abstract int ReceivePacket(NetworkSocket socket);
        
        protected abstract int SendPacket(object packet, NetworkSocket socket);
    }
}