using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine.SceneManagement;
using System;

namespace PawsAndClaws.Networking
{
    public class NetClient : MonoBehaviour
    {
        public static Action OnPacketReceived;
        public static Action OnPacketSend;
        
        public ProtocolType protocolType = NetworkData.ProtocolType;
        private EndPoint _endPoint;
        private Thread _thread;

        private byte[] _packetBytes = new byte[2048];
        private bool _connected = false;

        void Start()
        {
            IPHostEntry entry = Dns.GetHostEntry(Dns.GetHostName());
            _endPoint = NetworkData.ServerEndPoint;
            _thread = new Thread(UpdateData);
            _thread.Start();
            _connected = true;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseConnections();
                Application.Quit();
            }

            if (!_connected)
            {
                CloseConnections();
            }
        }
        private void UpdateData()
        {
            while (true)
            {
                int recv = NetworkData.NetSocket.Socket.ReceiveFrom(_packetBytes, ref _endPoint);
                OnPacketReceived?.Invoke();
                Debug.Log($"Client received packet from server with size {recv}");

                // The host disconnected
                if (recv <= 0)
                {
                    Debug.Log("Host disconnected return to main menu!");
                    _connected = false;
                    break;
                }
            }
        }

        private void CloseConnections()
        {
            if (_thread.IsAlive)
            {
                _thread.Abort();
                _thread = null;
            }

            _connected = false;

            NetworkData.NetSocket.Socket.Shutdown(SocketShutdown.Both);
            NetworkData.NetSocket.Socket.Close();

            Debug.Log("Closing connection socket.");

            // Return to the main menu to reconnect
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            // If the host disconnects the connections are already closed
            if (_connected)
                CloseConnections();
        }
    }
}