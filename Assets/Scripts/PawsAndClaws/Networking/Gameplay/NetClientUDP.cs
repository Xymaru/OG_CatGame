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
        private PacketManagerUDP packetManagerUDP = new PacketManagerUDP();
        private Socket socket;
        private bool connected = false;
        private NetworkPacket lastPacketReceived = null;

        private void Start()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            packetManagerUDP.OnPacketReceived += OnPacketReceived;
            packetManagerUDP.BeginReceive(socket);
            StartCoroutine(SendHelloPacket());
        }

        private void OnPacketReceived(NetworkPacket packet)
        {
            lastPacketReceived = packet;
        }
        private void Update()
        {
            ProcessPackets();
        }

        public void SendPacket(NetworkPacket packet)
        {
            packetManagerUDP.SendPacket(packet.ToByteArray());
        }

        private void ProcessPackets()
        {
            if (lastPacketReceived == null)
                return;

            // Wait for the server handshake to then start processing all the other packets
            if(lastPacketReceived.p_type == NPacketType.HELLO)
            {
                connected = true;
            }
            else if(connected)
            {
                ReplicationManager.Instance.ProcessPacket(lastPacketReceived);
            }
            // Clean the packet
            lastPacketReceived = null;
        }

        private IEnumerator SendHelloPacket()
        {
            while(connected) 
            {
                SendPacket(new NPHello());
                yield return new WaitForSeconds(NetworkData.PacketSendInterval);
            }
        }

        private void OnDestroy()
        {
            if(socket != null)
            {
                socket.Dispose();
            }
        }
    }
}