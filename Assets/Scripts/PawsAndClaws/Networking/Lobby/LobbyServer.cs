using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;

namespace PawsAndClaws
{
    public class LobbyServer : MonoBehaviour
    {
        NetServerTCP _netServerTCP;

        void Start()
        {
            _netServerTCP = FindObjectOfType<NetServerTCP>();

            NetworkManager.OnPacketReceived += OnPacketRecv;
        }
        
        void Update()
        {
            
        }

        void OnDestroy()
        {
            NetworkManager.OnPacketReceived -= OnPacketRecv;
        }

        private void OnLobbyReq(NPLobbyReq packet)
        {
            // Set name data on client
            _netServerTCP.ConnectedClients[packet.id].PlayerI.name = packet.name;
        }

        private void OnLobbySpotReq(NPLobbySpotReq packet)
        {
            // Set client on spot

        }

        private void OnPacketRecv(NetworkPacket packet)
        {
            switch (packet.p_type)
            {
                case NPacketType.LOBBYREQ:
                    OnLobbyReq((NPLobbyReq)packet);
                    break;
                case NPacketType.LOBBYSPOTREQ:
                    OnLobbySpotReq((NPLobbySpotReq)packet);
                    break;
            }
        }
    }
}
