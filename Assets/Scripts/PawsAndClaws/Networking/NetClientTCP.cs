using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public class NetClientTCP : MonoBehaviour
    {
        public PlayerInfo[] ConnectedClients = new PlayerInfo[6];

        PacketManagerTCP _packetManagerTCP = new PacketManagerTCP();

        private List<NetworkPacket> _packetList = new();
        private object _packetMutex = new();

        void Start()
        {
            ConnectedClients[NetworkData.NetSocket.PlayerI.client_id] = NetworkData.NetSocket.PlayerI;

            _packetManagerTCP.OnPacketReceived += OnReceivedPacket;
            _packetManagerTCP.OnSocketDisconnected += OnSocketDisconnect;

            // Begin receiving from server
            _packetManagerTCP.BeginReceive(NetworkData.NetSocket);
        }

        void Update()
        {
            lock (_packetMutex)
            {
                for(int i = 0; i < _packetList.Count; i++)
                {
                    NetworkManager.OnPacketReceived?.Invoke(_packetList[i]);
                }

                _packetList.Clear();
            }
        }

        void OnReceivedPacket(NetworkPacket packet)
        {
            lock (_packetMutex)
            {
                _packetList.Add(packet);
            }
        }

        void OnSocketDisconnect(NetworkSocket socket)
        {
            // Handle server disconnection

        }

        private void CloseConnections()
        {
            if (NetworkData.NetSocket.Socket.Connected)
            {
                NetworkData.NetSocket.Socket.Shutdown(SocketShutdown.Both);
            }

            NetworkData.NetSocket.Socket.Close();

            Debug.Log("Closing connection socket.");
        }

        private void OnDestroy()
        {
            CloseConnections();
        }
    }
}