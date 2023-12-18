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
        private PacketStateUDP stateObj = new PacketStateUDP();
        private bool connected = false;

        private void Start()
        {
            stateObj.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            stateObj.RemoteEP  = new IPEndPoint(NetworkData.NetSocket.IPAddr, NetworkData.PortUDP);
            stateObj.Buffer = new byte[NetworkPacket.MAX_BUFFER_SIZE];
            
            StartCoroutine(SendHelloPacket());
            BeginReceive();
        }

        private void BeginReceive()
        {
            stateObj.Socket.BeginReceiveFrom(stateObj.Buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, SocketFlags.None, 
                ref stateObj.RemoteEP, new AsyncCallback(ReceiveCB), stateObj);
        }

        private void ReceiveCB(IAsyncResult ar)
        {
            PacketStateUDP obj = (PacketStateUDP) ar.AsyncState;
            int bytesReceived = obj.Socket.EndReceiveFrom(ar, ref stateObj.RemoteEP);
            if (bytesReceived == 0)
                return;

            NetworkPacket packet = NetworkPacket.FromByteArray(stateObj.Buffer);
            if (packet.p_type == NPacketType.HELLO)
            {
                connected = true;
                Debug.Log($"Correctly connected to server UDP");
            }
            else
            {
                ReplicationManager.Instance.ProcessPacket(packet);
            }
            stateObj.Socket.BeginReceiveFrom(stateObj.Buffer, 0, NetworkPacket.MAX_BUFFER_SIZE, SocketFlags.None, 
                ref stateObj.RemoteEP, new AsyncCallback(ReceiveCB), stateObj);
        }
        
        public void SendPacket(NetworkPacket packet)
        {
            stateObj.Socket.SendTo(packet.ToByteArray(), SocketFlags.None, stateObj.RemoteEP);
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