using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

using TMPro;
using System;
using System.Text;

using UnityEngine.SceneManagement;
using PawsAndClaws.Networking;

namespace PawsAndClaws.UI
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject menuPanel;
        public GameObject connectPanel;

        public TMP_InputField ipinput;

        public void OnHostClick()
        {
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, NetworkData.Port);

            IPAddress hostIP = IPAddress.Any;

            IPAddress[] addresslist = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

            // Get last ipv4
            for(int i = 0; i < addresslist.Length; i++)
            {
                IPAddress ip = addresslist[addresslist.Length - i - 1];

                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    hostIP = ip;
                    break;
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

            Socket clientSocket = new Socket(ipaddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

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
            SceneManager.LoadScene("Lobby");
        }
    }
}