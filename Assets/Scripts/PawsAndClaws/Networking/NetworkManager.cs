using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    void Start()
    {
        if(NetworkData.netSocket.netcon == NetCon.CLIENT)
        {
            gameObject.AddComponent<NetClient>();
        }
        else if (NetworkData.protocolType == ProtocolType.Udp)
        {
            gameObject.AddComponent<NetServerUDP>();
        }
        else
        {
            gameObject.AddComponent<NetServerTCP>();
        }
    }
}
