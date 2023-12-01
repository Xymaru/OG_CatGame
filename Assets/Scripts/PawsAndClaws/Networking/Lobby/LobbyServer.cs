using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;

using System;

namespace PawsAndClaws
{
    public class LobbyServer : MonoBehaviour
    {
        NetServerTCP _netServerTCP;

        public Action<PlayerInfo, ushort, Player.Team> OnSlotUpdate;

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

            // New connection packet
            NPLobbyPlayerCon player_con = new NPLobbyPlayerCon();
            player_con.name = packet.name;
            player_con.client_id = packet.id;

            // Tell all clients about new connection
            _netServerTCP.BroadcastPacket(player_con);

            // Tell new connection about server
            player_con.client_id = 5;
            player_con.name = Game.GameConstants.UserName;

            byte[] data = player_con.ToByteArray();

            NetworkSocket netSocket = _netServerTCP.ConnectedClients[packet.id];
            netSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);

            // Tell new connection about all other clients
            foreach (NetworkSocket socket in _netServerTCP.ConnectedClients)
            {
                if (socket != null)
                {
                    player_con.client_id = socket.PlayerI.client_id;
                    player_con.name = socket.PlayerI.name;

                    data = player_con.ToByteArray();

                    netSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);
                }
            }
        }

        private void OnLobbySpotReq(NPLobbySpotReq packet)
        {
            PlayerInfo pinfo = NetworkData.Teams[(int)packet.team].members[packet.spot];

            // Check if player is on the spot
            if (pinfo != null) return;

            pinfo = _netServerTCP.ConnectedClients[packet.id].PlayerI;

            // Slot update callback
            OnSlotUpdate?.Invoke(pinfo, packet.spot, packet.team);

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
