using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;

using TMPro;
using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;

using System;

namespace PawsAndClaws
{
    public class LobbyUI : MonoBehaviour
    {
        void Start()
        {
            if(NetworkData.NetSocket.NetCon == NetCon.Client)
            {

            }
            else{
                gameObject.AddComponent<LobbyServer>();
            }
        }

        void Update()
        {
            
        }

        public void RequestSpotHamster(int index)
        {
            RequestSpot(index, Player.Team.Hamster);
        }

        public void RequestSpotCat(int index)
        {
            RequestSpot(index, Player.Team.Cat);
        }

        private void RequestSpot(int index, Player.Team team)
        {
            
        }

        private void RequestSpotClient(int index, Player.Team team)
        {
            NPLobbySpotReq spot_req = new NPLobbySpotReq();
            spot_req.id = NetworkData.NetSocket.PlayerI.client_id;

            spot_req.spot = Convert.ToUInt16(index);
            spot_req.team = team;

            byte[] data = spot_req.ToByteArray();

            NetworkData.NetSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);
        }

        private void RequestSpotServer(int index, Player.Team team)
        {
            
        }
    }
}
