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
            // Check if player is on the spot
            if (NetworkData.Teams[(int)packet.team].members[packet.spot] != null) return;

            // Otherwise, put it on spot
            NetworkData.Teams[(int)packet.team].members[packet.spot] = _netServerTCP.ConnectedClients[packet.id].PlayerI;

            // Make slot update packet
            NPLobbySpotUpdate spot_update = new NPLobbySpotUpdate();
            spot_update.id = packet.id;
            spot_update.spot = packet.spot;
            spot_update.team = packet.team;

            // Broadcast slot change to everyone
            _netServerTCP.BroadcastPacket(spot_update);
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
