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

        UdpClient socket = null;

        private bool connected = false;

        private void Awake()
        {
            int port = NetworkData.PortUDP + NetworkData.NetSocket.PlayerI.client_id;

            Debug.Log(port);

            // Port is 6969 + ID (1 to 5)
            socket = new UdpClient(port);

            // Connect to server
            socket.Connect("localhost", NetworkData.PortUDP);
        }

        private void Start()
        {
            NPHello p_hello = new NPHello();
            p_hello.id = NetworkData.NetSocket.PlayerI.client_id;

            SendPacket(p_hello);
            
            //StartCoroutine(SendHelloPacket());
            BeginReceive();
        }

        private void BeginReceive()
        {
            PacketStateUDP stateObj = new PacketStateUDP();
            
            stateObj.Socket = socket;
            stateObj.Buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];

            stateObj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), stateObj);
        }

        private void ReceiveCB(IAsyncResult ar)
        {
            PacketStateUDP obj = (PacketStateUDP) ar.AsyncState;
            obj.Buffer = obj.Socket.EndReceive(ar, ref obj.RemoteEP);

            NetworkPacket packet = NetworkPacket.FromByteArray(obj.Buffer);
            if (packet.p_type == NPacketType.HELLO)
            {
                connected = true;
                Debug.Log($"Correctly connected to server UDP");
            }
            else
            {
                ReplicationManager.Instance.ProcessPacket(packet);
            }

            obj.Socket.BeginReceive(new AsyncCallback(ReceiveCB), obj);
        }
        
        public void SendPacket(NetworkPacket packet)
        {
            socket.Send(packet.ToByteArray(), NetworkPacket.MAX_BUFFER_SIZE);
        }

       private IEnumerator SendHelloPacket()
        {
            while(!connected) 
            {
                SendPacket(new NPHello());
                yield return new WaitForSeconds(NetworkData.PacketSendInterval);
            }
        }
    }
}