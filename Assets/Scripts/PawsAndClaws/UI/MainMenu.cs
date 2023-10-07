using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

using TMPro;
using System;
using System.Text;

using UnityEngine.SceneManagement;

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

        NetworkData.protocolType = protocol;
    }
    public void OnHostClick()
    {
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, socketType, protocol);

        IPAddress hostIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

        IPEndPoint ep = new IPEndPoint(IPAddress.Any, NetworkData.PORT);

        listenSocket.Bind(ep);

        NetworkData.netSocket = new NetworkServerSocket(listenSocket, hostIP, hostIP.ToString());


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
        IPEndPoint remoteIP = new IPEndPoint(ipaddr, NetworkData.PORT);

        Socket clientSocket = new Socket(ipaddr.AddressFamily, socketType, protocol);


        if (protocol == ProtocolType.Tcp)
        {
            try
            {
                clientSocket.Connect(remoteIP);

                Debug.Log("Connected to " + clientSocket.RemoteEndPoint.ToString());
            }
            catch (Exception e)
            {
                Debug.Log("Error on connecting to server." + e.ToString());
                return;
            }
        }
        // After connection has been made, set data
        NetworkData.netSocket = new NetworkSocket(clientSocket, ipaddr, ipinput.text);

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
