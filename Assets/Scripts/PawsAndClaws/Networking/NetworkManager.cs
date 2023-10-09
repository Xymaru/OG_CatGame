using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        public TMPro.TextMeshProUGUI ipsText;
        public TMPro.TextMeshProUGUI lobbyInfoText;
        void Start()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                gameObject.AddComponent<NetClient>();
                gameObject.AddComponent<NetChatClient>();
                ipsText.gameObject.SetActive(false);
                SetClientInfo();
            }
            else if (NetworkData.ProtocolType == ProtocolType.Udp)
            {
                gameObject.AddComponent<NetServerUDP>();
                gameObject.AddComponent<NetChatServer>();
                SetServerInfo();
            }
            else
            {
                gameObject.AddComponent<NetServerTCP>();
                gameObject.AddComponent<NetChatServer>();
                SetServerInfo();
            }
        }

        public void SetServerInfo()
        {
            lobbyInfoText.text = $"Server \nHosting at IP: {NetworkData.NetSocket.IPAddr.MapToIPv4()} \nConnection mode {NetworkData.ProtocolType.ToString()}";
        }

        public void SetClientInfo()
        {
            lobbyInfoText.text = $"Client \nConnected to IP: {NetworkData.NetSocket.Socket.RemoteEndPoint} \nConnection mode {NetworkData.ProtocolType.ToString()}";
        }
    }
}