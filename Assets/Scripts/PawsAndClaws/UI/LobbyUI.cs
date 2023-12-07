using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;

using TMPro;
using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;

using System;

using PawsAndClaws.UI;

namespace PawsAndClaws
{
    public class SlotChangeStats
    {
        public PlayerInfo playerInfo;
        public ushort slot;
        public Player.Team team;

        public SlotChangeStats(PlayerInfo pinfo, ushort sl, Player.Team t)
        {
            playerInfo = pinfo;
            slot = sl;
            team = t;
        }
    }

    public class LobbyUI : MonoBehaviour
    {
        public GameObject start_btn;
        public TeamSlotUI[] cat_slots = new TeamSlotUI[3];
        public TeamSlotUI[] hamster_slots = new TeamSlotUI[3];

        void Start()
        {
            if(NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                LobbyClient _cl = gameObject.AddComponent<LobbyClient>();
                _cl.OnSlotUpdate += OnSlotChange;
                _cl.OnUserReadyUpdate += OnUserReady;
            }
            else{
                LobbyServer _srv = gameObject.AddComponent<LobbyServer>();
                _srv.OnSlotUpdate += OnSlotChange;
                _srv.OnUserReadyUpdate += OnUserReady;

                start_btn.SetActive(true);
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

        public void RequestReady()
        {
            PlayerInfo pinfo = NetworkData.NetSocket.PlayerI;

            if(pinfo.slot != ushort.MaxValue)
            {
                pinfo.is_ready = !pinfo.is_ready;

                NPLobbyReadyReq req_ready = new NPLobbyReadyReq();
                req_ready.id = pinfo.client_id;
                req_ready.is_ready = pinfo.is_ready;

                // Request ready
                if (NetworkData.NetSocket.NetCon == NetCon.Client)
                {
                    byte[] data = req_ready.ToByteArray();

                    NetworkData.NetSocket.Socket.Send(data, NetworkPacket.MAX_BUFFER_SIZE, 0);
                }
                else
                {
                    OnUserReady(pinfo);

                    // Broadcast packet to all clients
                    FindObjectOfType<NetServerTCP>().BroadcastPacket(req_ready);
                }
            }
        }

        private void RequestSpot(int index, Player.Team team)
        {
            if (NetworkData.NetSocket.PlayerI.is_ready) return;

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

                OnSlotChange(info, Convert.ToUInt16(index), team);

                NPLobbySpotUpdate spot_update = new NPLobbySpotUpdate();
                spot_update.id = NetworkData.NetSocket.PlayerI.client_id;
                spot_update.spot = info.slot;
                spot_update.team = info.team;

                // Broadcast packet to all clients
                FindObjectOfType<NetServerTCP>().BroadcastPacket(spot_update);
            }
        }

        private void OnSlotChange(PlayerInfo info, ushort new_slot, Player.Team new_team)
        {
            // Remove previous position
            if (info.slot != ushort.MaxValue)
            {
                NetworkData.Teams[(int)info.team].members[info.slot] = null;

                if (info.team == Player.Team.Cat)
                {
                    cat_slots[info.slot].OnUserRemove();
                }
                else
                {
                    hamster_slots[info.slot].OnUserRemove();
                }
            }
            
            // Set info
            info.slot = new_slot;
            info.team = new_team;

            NetworkData.Teams[(int)new_team].members[new_slot] = info;

            if (info.team == Player.Team.Cat)
            {
                cat_slots[info.slot].OnUserChange(info.name);
            }
            else
            {
                hamster_slots[info.slot].OnUserChange(info.name);
            }
        }

        private void OnUserReady(PlayerInfo pinfo)
        {
            if (pinfo.team == Player.Team.Cat)
            {
                cat_slots[pinfo.slot].SetUserReady(pinfo.is_ready);
            }
            else
            {
                hamster_slots[pinfo.slot].SetUserReady(pinfo.is_ready);
            }
        }

        public void StartGame()
        {
            NPLobbyStartGame start_game = new NPLobbyStartGame();

            // Broadcast packet to all clients
            FindObjectOfType<NetServerTCP>().BroadcastPacket(start_game);

            // Change scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
    }
}
