using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ipsText;
    public TMPro.TextMeshProUGUI lobbyInfoText;
    void Start()
    {
        if(NetworkData.NetSocket.NetCon == NetCon.Client)
        {
            gameObject.AddComponent<NetClient>();
            ipsText.gameObject.SetActive(false);
            SetClientInfo();
        }
        else if (NetworkData.ProtocolType == ProtocolType.Udp)
        {
            gameObject.AddComponent<NetServerUDP>();
            SetServerInfo();
        }
        else
        {
            gameObject.AddComponent<NetServerTCP>();
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
