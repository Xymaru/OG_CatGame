using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public abstract class NetServer : MonoBehaviour
    {
        public List<NetworkSocket> ConnectedClients = new();

        public static Action<NetworkSocket> OnConnectionAccept;
        public static Action<NetworkSocket> OnClientDisconnect;

        protected NetworkServerSocket _serverSocket;
        protected NetworkManager _networkManager;

        private void Awake()
        {
            _serverSocket = (NetworkServerSocket)NetworkData.NetSocket;
            _networkManager = GetComponent<NetworkManager>();
        }
    }
}