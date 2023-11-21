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
        public NPPlayerPos paket2;

        void Start()
        {
            NetServer.OnPacketReceived += OnPacketRecv;
        }
        
        void Update()
        {
            
        }

        void OnDestroy()
        {
            NetServer.OnPacketReceived -= OnPacketRecv;
        }

        private void OnPacketRecv(NetworkPacket packet)
        {
            //if (packet.p_type != NPacketType.LOBBYREQ) return;

            if (packet.p_type == NPacketType.LOBBYREQ)
            {
                NPLobbyReq req_packet = (NPLobbyReq)packet;

                paket = req_packet;
            }else if(packet.p_type == NPacketType.PLAYERPOS)
            {
                NPPlayerPos req_packet = (NPPlayerPos)packet;

                paket2 = req_packet;
            }
        }
    }
}
