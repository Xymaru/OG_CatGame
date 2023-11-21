using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        void Awake()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                gameObject.AddComponent<NetClient>();
            }
            else
            {
                gameObject.AddComponent<NetServerTCP>();
            }
        }


        public void SendReadyPacket()
        {
            
        }
    }
}