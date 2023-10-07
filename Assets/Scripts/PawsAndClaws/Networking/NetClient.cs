using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

using System.Net.Sockets;
using System.Net;
using System.Threading;

public class NetClient : MonoBehaviour
{
    public ProtocolType protocolType = NetworkData.ProtocolType;
    private EndPoint _endPoint;
    private Thread _thread;
    private byte[] _data = new byte[1024];
    void Start()
    {
        IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
        _endPoint = NetworkData.ServerEndPoint;
        _thread = new Thread(UpdateData);
        _thread.Start();
    }

    void Update()
    {
        if (protocolType == ProtocolType.Tcp)
        {
            InputTCP();
        }
        else
        {
            InputUDP();
        }
    }
    private void UpdateData()
    {
        while (true)
        {
            int recv = NetworkData.NetSocket.Socket.ReceiveFrom(_data, ref _endPoint);
            string msg = Encoding.ASCII.GetString(_data, 0, recv);
            Debug.Log($"Client recieved from server [{msg}]");
        }
    }

    private void InputUDP()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] buff = Encoding.ASCII.GetBytes("Hola xikilicuatre");
            Debug.Log("Sending packet");
            NetworkData.NetSocket.Socket.SendTo(buff, buff.Length, SocketFlags.None, _endPoint);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NetworkData.NetSocket.Socket.Shutdown(SocketShutdown.Both);
            Application.Quit();
        }
    }
    private void InputTCP()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] buff = Encoding.ASCII.GetBytes("Hola xikilicuatre");

            NetworkData.NetSocket.Socket.Send(buff);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseConnections();
            Application.Quit();
        }
    }

    private void CloseConnections()
    {
        if (_thread.IsAlive)
        {
            _thread.Abort();
            _thread = null;
        }
        NetworkData.NetSocket.Socket.Shutdown(SocketShutdown.Both);
        NetworkData.NetSocket.Socket.Close();

        Debug.Log("Closing connection socket.");
    }

    private void OnDestroy()
    {
        CloseConnections();
    }
}