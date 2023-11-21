using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;

using TMPro;
using System;
using System.Text;
using PawsAndClaws.Game;
using UnityEngine.SceneManagement;
using PawsAndClaws.Networking;
using PawsAndClaws.Networking.Packets;
using PawsAndClaws.Scenes;
using UnityEngine.Serialization;

namespace PawsAndClaws.UI
{
    public class OnlineMatchCreator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField ipInput;

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
        
        public void MakeConnection()
        {
            // Check if IP is not empty
            if (ipInput.text == "")
            {
                Debug.Log("Trying to connect to an empty IP address.");
                return;
            }

            // Parse ip address
            IPAddress ipaddr;

            bool valid = IPAddress.TryParse(ipInput.text, out ipaddr);

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
            NetworkData.NetSocket = new NetworkSocket(clientSocket, ipaddr, ipInput.text);
            EnterLobby();
            OpenLobby();
        }
        
        private void EnterLobby()
        {
            //if (_sentReq) return;
            NPLobbyReq nlobreq = new NPLobbyReq();
            nlobreq.name = GameConstants.UserName;
            byte[] data = LobbyNetworkPacket.NPLobbyReqToByteArray(nlobreq);

            int bytes_sent = NetworkData.NetSocket.Socket.Send(data);

            Debug.Log($"Sent packet with {bytes_sent} bytes.");
        }
        
        private void OpenLobby()
        {
            SceneTransitionManager.TransitionTo?.Invoke(1);
        }
    }
}