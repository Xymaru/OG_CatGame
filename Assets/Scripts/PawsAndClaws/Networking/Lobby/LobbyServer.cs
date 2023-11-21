using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;

namespace PawsAndClaws
{
    public class LobbyServer : MonoBehaviour
    {
        public NPLobbyReq paket;

        void Start()
        {
            NetServer.OnPacketReceived += OnPacketRecv;
        }

        void OnDestroy()
        {
            NetServer.OnPacketReceived -= OnPacketRecv;
        }

        private void OnPacketRecv(NetworkPacket packet)
        {
            NPacketType p_type = (NPacketType)packet.p_type;

            if (p_type != NPacketType.LOBBYREQ) return;

            NPLobbyReq req_packet = (NPLobbyReq)packet;

            paket = req_packet;
        }
    }
}
