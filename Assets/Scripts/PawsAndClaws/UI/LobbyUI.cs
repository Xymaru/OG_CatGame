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
        public TMP_Text[] hamsters_texts = new TMP_Text[3];
        public TMP_Text[] cats_texts = new TMP_Text[3];

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
            for(int i = 0; i < 3; i++)
            {
                PlayerInfo c_info = NetworkData.Teams[(int)Player.Team.Cat].members[i];
                if (c_info != null)
                {
                    cats_texts[i].text = c_info.name;
                }
                else
                {
                    cats_texts[i].text = "Empty slot";
                }

                PlayerInfo h_info = NetworkData.Teams[(int)Player.Team.Hamster].members[i];
                if (h_info != null)
                {
                    hamsters_texts[i].text = h_info.name;
                }
                else
                {
                    hamsters_texts[i].text = "Empty slot";
                }
            }
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
            if (NetworkData.Teams[(int)team].members[index] != null) return;

            if(NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                RequestSpotClient(index, team);
            }
            else
            {
                RequestSpotServer(index, team);
            }
        }

        private void RequestSpotClient(int index, Player.Team team)
        {
            NPLobbySpotReq spot_req = new NPLobbySpotReq();
            spot_req.id = NetworkData.NetSocket.PlayerI.client_id;

            spot_req.spot = Convert.ToUInt16(index);
            spot_req.team = team;

            byte[] data = spot_req.ToByteArray();

            // Request spot
            NetworkData.NetSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);
        }

        private void RequestSpotServer(int index, Player.Team team)
        {
            if (NetworkData.Teams[(int)team].members[index] == null)
            {
                PlayerInfo info = NetworkData.NetSocket.PlayerI;
                info.slot = Convert.ToUInt16(index);
                info.team = team;

                NetworkData.Teams[(int)team].members[index] = info;

                NPLobbySpotUpdate spot_update = new NPLobbySpotUpdate();
                spot_update.id = NetworkData.NetSocket.PlayerI.client_id;
                spot_update.spot = info.slot;
                spot_update.team = info.team;

                // Broadcast packet to all clients
                FindObjectOfType<NetServerTCP>().BroadcastPacket(spot_update);
            }
        }
    }
}
