using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public enum NetCon
{
    CLIENT,
    HOST
}

public class NetworkSocket
{
    public string ip_addr_str = "localhost";
    public NetCon netcon = NetCon.CLIENT;
    public IPAddress ip_addr;
    public Socket socket;

    public NetworkSocket(Socket s, IPAddress ipaddr, string ipstr)
    {
        socket = s;
        ip_addr_str = ipstr;
        ip_addr = ipaddr;
    }
}

public class NetworkServerSocket : NetworkSocket
{
    public Socket[] connectedSockets;

    public NetworkServerSocket(Socket s, IPAddress ipaddr, string ipstr) : base(s, ipaddr, ipstr)
    {
        netcon = NetCon.HOST;
    }
}

public static class NetworkData
{
    public const int PORT = 50420;

    public static NetworkSocket netSocket;

    public static string public_name = "Player";
}