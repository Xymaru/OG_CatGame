using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public enum NetCon
{
    Client,
    Host
}

public class NetworkSocket
{
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
    public Socket[] ConnectedSockets;

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

    public static string PublicName = "Player";
}