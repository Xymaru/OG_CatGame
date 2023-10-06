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

    public void OnHostClick()
    {
        Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPAddress hostIP = IPAddress.Parse("127.0.0.1");

        IPEndPoint ep = new IPEndPoint(hostIP, NetworkData.PORT);

        listenSocket.Bind(ep);

        NetworkData.netSocket = new NetworkServerSocket(listenSocket, hostIP, hostIP.ToString());

        SceneManager.LoadScene("Lobby");
    }

    public void OnConnectClick()
    {
        menuPanel.SetActive(false);
        connectPanel.SetActive(true);
    }

    public void MakeConnection()
    {
        // Check if IP is not empty
        if(ipinput.text == "")
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

        Socket clientSocket = new Socket(ipaddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            clientSocket.Connect(remoteIP);

            Debug.Log("Connected to " + clientSocket.RemoteEndPoint.ToString());
        }
        catch(Exception e)
        {
            Debug.Log("Error on connecting to server." + e.ToString());
            return;
        }

        // After connection has been made, set data
        NetworkData.netSocket = new NetworkSocket(clientSocket, ipaddr, ipinput.text);

        // Go to lobby
        SceneManager.LoadScene("Lobby");
    }
}
