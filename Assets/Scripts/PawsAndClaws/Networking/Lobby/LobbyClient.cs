using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using PawsAndClaws.Networking.Packets;

namespace PawsAndClaws.Networking {
    public class LobbyClient : MonoBehaviour
    {
        NetClientTCP _netClientTCP;

        public Action<PlayerInfo, ushort, Player.Team> OnSlotUpdate;

        bool _startGame = false;

        void Start()
        {
            _netClientTCP = FindObjectOfType<NetClientTCP>();

            NetworkManager.OnPacketReceived += OnPacketRecv;
        }

        void Update()
        {
            if (_startGame)
            {
                // Change scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
            }
        }

        private void SpotUpdate(NPLobbySpotUpdate spot_update)
        {
            PlayerInfo pinfo = null;

            // Check if it's my spot
            if (spot_update.id == NetworkData.NetSocket.PlayerI.client_id)
            {
                pinfo = NetworkData.NetSocket.PlayerI;
            }
            // Else, it's someone else
            else
            {
                pinfo = _netClientTCP.ConnectedClients[spot_update.id];
            }

            if (pinfo == null) return;

            OnSlotUpdate?.Invoke(pinfo, spot_update.spot, spot_update.team);
        }

        private void OnLobbyPlayerCon(NPLobbyPlayerCon packet)
        {
            if (packet.client_id == NetworkData.NetSocket.PlayerI.client_id) return;

            PlayerInfo pinfo = new PlayerInfo();
            pinfo.name = packet.name;
            pinfo.client_id = packet.client_id;

            _netClientTCP.ConnectedClients[packet.client_id] = pinfo;

            Debug.Log($"New player connected with name {pinfo.name} and ID {pinfo.client_id}");
        }

        private void OnLobbyStartGame(NPLobbyStartGame packet)
        {
            _startGame = true;
        }

        private void OnPacketRecv(NetworkPacket packet)
        {
            switch (packet.p_type)
            {
                case NPacketType.LOBBYSPOTUPDATE:
                    SpotUpdate((NPLobbySpotUpdate)packet);
                    break;
                case NPacketType.LOBBYPLAYERCON:
                    OnLobbyPlayerCon((NPLobbyPlayerCon)packet);
                    break;
                case NPacketType.LOBBYSTARTGAME:
                    OnLobbyStartGame((NPLobbyStartGame)packet);
                    break;
            }
        }

        private void OnDestroy()
        {
            NetworkManager.OnPacketReceived -= OnPacketRecv;
        }
    }
}