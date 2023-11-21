using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;

using TMPro;
using PawsAndClaws.Networking;

namespace PawsAndClaws
{
    public class LobbyUI : MonoBehaviour
    {
        private bool _sentReq = false;

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

        public void EnterLobby()
        {
            //if (_sentReq) return;

            string name = _nameInput.text;

            NPLobbyReq nlobreq = new NPLobbyReq();
            nlobreq.name = name;

            byte[] data = LobbyNetworkPacket.NPLobbyReqToByteArray(nlobreq);

            int bytes_sent = NetworkData.NetSocket.Socket.Send(data);

            Debug.Log($"Sent name packet with {bytes_sent} bytes.");

            NPPlayerPos nplayerpos = new NPPlayerPos();
            nplayerpos.id = 6;
            nplayerpos.x = Random.Range(-20.0f, 20.0f);
            nplayerpos.y = Random.Range(-20.0f, 20.0f);

            data = GameplayNetworkPacket.NPPlayerPosToByteArray(nplayerpos);

            bytes_sent = NetworkData.NetSocket.Socket.Send(data);

            Debug.Log($"Sent pos packet ({nplayerpos.x},{nplayerpos.y}) with {bytes_sent} bytes.");

            _sentReq = true;
        }
    }
}
