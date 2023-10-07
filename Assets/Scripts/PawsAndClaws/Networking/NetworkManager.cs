using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ipsText;
    void Start()
    {
        if(NetworkData.NetSocket.NetCon == NetCon.Client)
        {
            gameObject.AddComponent<NetClient>();
        }
        else if (NetworkData.ProtocolType == ProtocolType.Udp)
        {
            gameObject.AddComponent<NetServerUDP>();
        }
        else
        {
            gameObject.AddComponent<NetServerTCP>();
        }
    }
}
