using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using PawsAndClaws.Networking;

namespace PawsAndClaws
{
    public class LobbyUI : MonoBehaviour
    {
        public TMP_InputField _nameInput;

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

            Debug.Log($"Sent packet with {bytes_sent} bytes.");

            _sentReq = true;
        }
    }
}
