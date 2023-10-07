using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

using System.Net.Sockets;
using System.Net;
using System.Threading;

public class NetClient : MonoBehaviour
{
    public ProtocolType protocolType = NetworkData.protocolType;
    private EndPoint _endPoint;
    private Thread _thread;
    private byte[] _data = new byte[1024];
    void Start()
    {
        IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());

        _endPoint = new IPEndPoint(entry.AddressList[0], NetworkData.PORT);
        _thread = new Thread(UpdateData);
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
            int recv = NetworkData.netSocket.socket.ReceiveFrom(_data, ref _endPoint);
            string msg = Encoding.ASCII.GetString(_data, 0, recv);
            Debug.Log($"Client recieved from server [{msg}]");

        }
    }

    private void InputUDP()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] buff = Encoding.ASCII.GetBytes("Hola xikilicuatre");

            NetworkData.netSocket.socket.SendTo(buff, _endPoint);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NetworkData.netSocket.socket.Shutdown(SocketShutdown.Both);

            Application.Quit();
        }
    }
    private void InputTCP()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] buff = Encoding.ASCII.GetBytes("Hola xikilicuatre");

            NetworkData.netSocket.socket.Send(buff);
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
        NetworkData.netSocket.socket.Shutdown(SocketShutdown.Both);
        NetworkData.netSocket.socket.Close();

        Debug.Log("Closing connection socket.");
    }

    private void OnDestroy()
    {
        CloseConnections();
    }
}
