using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetServerUDP : MonoBehaviour
{
    private NetworkServerSocket _serverSocket;
    private Thread _updateThread;

    private byte[] _data = new byte[1024];
    public static Action<string> OnMessageReceived; 
    public void Start()
    {
        _serverSocket = (NetworkServerSocket)NetworkData.NetSocket;
        _updateThread = new Thread(UpdateThread);
        _updateThread.Start();
    }

    void UpdateThread()
    {
        EndPoint endPoint = new IPEndPoint(IPAddress.Any, NetworkData.Port);
        while (true)
        {
            int revSize = _serverSocket.Socket.ReceiveFrom(_data, ref endPoint);
            string msg = Encoding.ASCII.GetString(_data, 0, revSize);
            OnMessageReceived?.Invoke(msg);
        }
    }

    private void OnDestroy()
    {
        if (_updateThread.IsAlive)
        {
            _updateThread.Abort();

            _serverSocket.Socket.Shutdown(SocketShutdown.Both);
            _serverSocket.Socket.Close();
        }
    }
}
