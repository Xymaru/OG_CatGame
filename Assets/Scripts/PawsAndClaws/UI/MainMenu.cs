using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

using TMPro;
using System;
using System.Text;

using UnityEngine.SceneManagement;
using PawsAndClaws.Encrypting;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject connectPanel;

    public TMP_InputField ipinput;

    public ProtocolType protocol = ProtocolType.Tcp;
    public SocketType socketType = SocketType.Stream;

    public void OnProtocolChanged(int value)
    {
        // 0 TCP
        // 1 UDP

        switch (value)
        {
            case 0:
                {
                    protocol = ProtocolType.Tcp;
                    socketType = SocketType.Stream;
                }
                break;
            case 1:
                {
                    protocol = ProtocolType.Udp;
                    socketType = SocketType.Dgram;
                }
                break;
        }

        NetworkData.ProtocolType = protocol;
    }
    public void OnHostClick()
    {
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, socketType, protocol);

        
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, NetworkData.Port);

        IPAddress hostIP = IPAddress.Any;
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            // Get the last IP as is always the local IP
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                hostIP = ip;
            }
        }
       
        listenSocket.Bind(ep);

        NetworkData.NetSocket = new NetworkServerSocket(listenSocket, hostIP, hostIP.ToString());


        OpenLobby();
    }

    public void OnConnectClick()
    {
        menuPanel.SetActive(false);
        connectPanel.SetActive(true);
    }

    public void MakeConnection()
    {
        // Check if IP is not empty
        if (ipinput.text == "")
        {
            Debug.Log("Trying to connect to an empty IP address.");
            return;
        }

        // Parse ip address
        IPAddress ipaddr;

        bool valid = IPAddress.TryParse(ipinput.text, out ipaddr);

        if (!valid)
        {
            Debug.Log("Couldn't parse ip address.");
            return;
        }

        // Make connection
        NetworkData.ServerEndPoint = new IPEndPoint(ipaddr, NetworkData.Port);

        Socket clientSocket = new Socket(ipaddr.AddressFamily, socketType, protocol);

        
        try
        {
            clientSocket.Connect(NetworkData.ServerEndPoint);
            Debug.Log("Connected to " + clientSocket.RemoteEndPoint.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Error on connecting to server." + e.ToString());
            return;
        }
        // After connection has been made, set data
        NetworkData.NetSocket = new NetworkSocket(clientSocket, ipaddr, ipinput.text);

        OpenLobby();
    }

    private void OpenLobby()
    {
        // Go to lobby
        if (protocol == ProtocolType.Tcp)
        {
            SceneManager.LoadScene("LobbyTCP");
        }
        else
        {
            SceneManager.LoadScene("LobbyUDP");
        }
    }
}
