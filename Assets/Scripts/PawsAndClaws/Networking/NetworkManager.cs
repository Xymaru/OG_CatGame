using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using System;

namespace PawsAndClaws.Networking
{
    public enum ResponseType : ushort
    {
        ACCEPTED,
        LOBBY_FULL
    }

    public class NetworkManager : MonoBehaviour
    {
        public static Action<NetworkPacket> OnPacketReceived;
        public static Action<NetworkPacket> OnPacketSend;

        void Awake()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                gameObject.AddComponent<NetClientTCP>();
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