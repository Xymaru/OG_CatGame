using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using UnityEngine.SceneManagement;
using System;
using System.IO;

namespace PawsAndClaws.Networking
{
    public class NetClient : MonoBehaviour
    {
        public Action onPacketReceived;
        public Action onPacketSend;
        
        public ProtocolType protocolType = NetworkData.ProtocolType;
        private EndPoint _endPoint;
        private Thread _thread;

        public byte[] PacketBytes { get; private set; } = new byte[2048];
        private bool _connected = false;

        void Start()
        {
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
                int bytesReceived = ReceivePacket();

                // The host disconnected
                if (bytesReceived <= 0)
                {
                    Debug.Log("Host disconnected return to main menu!");
                    _connected = false;
                    break;
                }
            }
        }

        private int ReceivePacket()
        {
            int bytesReceived = NetworkData.NetSocket.Socket.ReceiveFrom(PacketBytes, ref _endPoint);
            var targetStream = new MemoryStream();

            NetPacket packet = Utils.BinaryUtils.ByteArrayToObject<NetPacket>(PacketBytes);

            while (bytesReceived < packet.size)
            {
                int bytes = NetworkData.NetSocket.Socket.ReceiveFrom(PacketBytes, ref _endPoint);
                bytesReceived += bytes;
                targetStream.Write(PacketBytes, 0, bytes);
                Debug.Log($"Writting bytes {bytes}");
            }
            Debug.Log($"Client received packet from server with size {bytesReceived}");
            
            PacketBytes = targetStream.ToArray();
            onPacketReceived?.Invoke();
            return bytesReceived;
        }

        public void SendPacket(NetPacket packet)
        {
            PacketBytes = Utils.BinaryUtils.ObjectToByteArray(packet);
            NetworkData.NetSocket.Socket.Send(PacketBytes);
            onPacketSend?.Invoke();
        }
        private void CloseConnections()
        {
            if (_thread.IsAlive)
            {
                _thread.Abort();
                _thread = null;
            }

            _connected = false;

            if (NetworkData.NetSocket.Socket.Connected)
            {
                NetworkData.NetSocket.Socket.Shutdown(SocketShutdown.Both);
            }

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