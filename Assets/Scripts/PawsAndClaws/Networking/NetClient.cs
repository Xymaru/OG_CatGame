using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine.SceneManagement;


namespace PawsAndClaws.Networking
{
    public class NetClient : MonoBehaviour
    {
        public ProtocolType protocolType = NetworkData.ProtocolType;
        private EndPoint _endPoint;
        private Thread _thread;

        private byte[] _packetBytes = new byte[1024];
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

                // The host disconnected
                if (recv <= 0)
                {
                    Debug.Log("Host disconnected return to main menu!");
                    _connected = false;
                    break;
                }

                string msg = Encoding.ASCII.GetString(_packetBytes, 0, recv);
                Debug.Log($"Client recieved from server [{msg}]");
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