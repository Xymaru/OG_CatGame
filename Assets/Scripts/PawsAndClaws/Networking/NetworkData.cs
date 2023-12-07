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
        public PlayerInfo PlayerI = new PlayerInfo();
        public string IPAddrStr = "localhost";
        public NetCon NetCon = NetCon.Client;
        public IPAddress IPAddr;
        public Socket Socket;
        public EndPoint EndPoint;
        public NetworkSocket(Socket s, IPAddress ipaddr, string ipStr, EndPoint endPoint)
        {
            Socket = s;
            IPAddrStr = ipStr;
            IPAddr = ipaddr;
            EndPoint = endPoint;
        }
    }

    public class NetworkServerSocket : NetworkSocket
    {
        public NetworkServerSocket(Socket s, IPAddress ipaddr, string ipStr) : base(s, ipaddr, ipStr, null)
        {
            NetCon = NetCon.Host;
        }
    }

    public static class NetworkData
    {
        public const int Port = 3154;
        public const int PortUDP = 6969;
        public static NetworkSocket NetSocket;

        public static EndPoint ServerEndPoint;

        public static ProtocolType ProtocolType = ProtocolType.Tcp;

        public static TeamInfo[] Teams = new TeamInfo[2] { new TeamInfo(), new TeamInfo() };
    }

    [System.Serializable]
    public class TeamInfo
    {
        public PlayerInfo[] members = new PlayerInfo[3];
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public string name = "Unknown";
        public ushort client_id = ushort.MaxValue;

        public ushort slot = ushort.MaxValue;
        public Player.Team team = Player.Team.None;
        public bool is_ready = false;

        public ushort character_id;
        public GameObject player_obj;
    }
}