using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;

namespace PawsAndClaws.Networking
{
    public class NetClientUDP : MonoBehaviour
    {
        UdpClient m_Socket = null;
        IPEndPoint m_ServerIPEP = null;

        List<NetworkPacket> m_PacketQueue = new();
        object m_PacketMutex = new();

        List<NetworkPacket> m_SendPacketQueue = new();
        object m_SendPacketMutex = new();

        bool connected = false;
        object m_ConMutex = new();

        private void Awake()
        {
            int port = NetworkData.PortUDP + NetworkData.NetSocket.PlayerI.client_id;

            // Port is 6969 + ID (1 to 5)
            m_Socket = new UdpClient(port);

            m_ServerIPEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), NetworkData.PortUDP);

            // Connect to server
            m_Socket.Connect(m_ServerIPEP);
        }

        private void Start()
        {
            NPHello p_hello = new NPHello();
            p_hello.id = NetworkData.NetSocket.PlayerI.client_id;
            
            // Register on server
            StartCoroutine(SendHelloPacket());

            BeginReceive();
        }

        private void Update()
        {
            lock (m_PacketMutex)
            {
                foreach (NetworkPacket p in m_PacketQueue)
                {
                    ReplicationManager.Instance.ProcessPacket(p);
                }

                m_PacketQueue.Clear();
            }

            lock (m_SendPacketMutex)
            {
                foreach (NetworkPacket p in m_SendPacketQueue)
                {
                    m_Socket.Send(p.ToByteArray(), NetworkPacket.MAX_BUFFER_SIZE);
                }

                m_SendPacketQueue.Clear();
            }
        }

        private void BeginReceive()
        {
            PacketStateUDP stateObj = new PacketStateUDP();
            
            stateObj.Socket = m_Socket;
            stateObj.Buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];

            stateObj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), stateObj);
        }

        private void ReceiveCB(IAsyncResult ar)
        {
            Debug.Log("Before data");

            PacketStateUDP obj = (PacketStateUDP) ar.AsyncState;

            Debug.Log("ASYNC");

            obj.RemoteEP = m_ServerIPEP;

            Debug.Log("REMOTEP");

            obj.Buffer = obj.Socket.EndReceive(ar, ref obj.RemoteEP);

            Debug.Log("BUFFER");

            NetworkPacket packet = NetworkPacket.FromByteArray(obj.Buffer);

            Debug.Log("PACKET");

            Debug.Log($"Received packet with ID {packet.p_type}");

            if (packet.p_type == NPacketType.HELLO)
            {
                lock (m_ConMutex)
                {
                    connected = true;
                }

                Debug.Log($"Correctly connected to server UDP");
            }
            else
            {
                lock (m_PacketMutex)
                {
                    m_PacketQueue.Add(packet);
                }
            }

            obj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), obj);
        }
        
        public void SendPacket(NetworkPacket packet)
        {
            lock (m_SendPacketMutex)
            {
                m_SendPacketQueue.Add(packet);
            }
        }

       private IEnumerator SendHelloPacket()
       {
            bool con = false;

            while(!con) 
            {
                NPHello p_hello = new();
                p_hello.id = NetworkData.NetSocket.PlayerI.client_id;

                SendPacket(p_hello);

                lock (m_ConMutex)
                {
                    con = connected;
                }

                // Wait till next try
                yield return new WaitForSeconds(NetworkData.PacketSendInterval);
            }
        }
    }
}