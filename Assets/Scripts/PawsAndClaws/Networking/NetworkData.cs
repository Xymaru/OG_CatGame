using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public enum NetCon
    {
        Client,
        Host
    }

    [System.Serializable]
    public class NetworkSocket
    {
        public string Name = "Unknown";
        public string IPAddrStr = "localhost";
        public NetCon NetCon = NetCon.Client;
        public IPAddress IPAddr;
        public Socket Socket;

        public NetworkSocket(Socket s, IPAddress ipaddr, string ipStr)
        {
            Socket = s;
            IPAddrStr = ipStr;
            IPAddr = ipaddr;
        }
    }

    public class NetworkServerSocket : NetworkSocket
    {
        public NetworkServerSocket(Socket s, IPAddress ipaddr, string ipStr) : base(s, ipaddr, ipStr)
        {
            NetCon = NetCon.Host;
        }
    }

    public static class NetworkData
    {
        public const int Port = 3154;

        public static NetworkSocket NetSocket;

        public static EndPoint ServerEndPoint;

        public static ProtocolType ProtocolType = ProtocolType.Tcp;

        public static ushort ClientID = 0;

        public static PlayerInfo playerInfo;

        public static PlayerInfo[] Cat_Team = new PlayerInfo[3];

        public static PlayerInfo[] Hamster_Team = new PlayerInfo[3];
    }

    public class PlayerInfo
    {
        public string name = "Player";
    }
}