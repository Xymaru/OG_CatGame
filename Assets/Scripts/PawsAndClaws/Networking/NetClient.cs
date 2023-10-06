using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;

using System.Net.Sockets;

public class NetClient : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] buff = Encoding.ASCII.GetBytes("Hola xikilicuatre");

            NetworkData.netSocket.socket.Send(buff);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            NetworkData.netSocket.socket.Shutdown(SocketShutdown.Both);

            Application.Quit();
        }
    }
}
