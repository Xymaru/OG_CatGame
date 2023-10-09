using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetChatServer : MonoBehaviour, INetChat
    {
        public static Action<string> OnMessageAdd;

        private NetServer _server;
        private List<string> _chatMessages = new List<string>();
        private object _mutex = new object();

        private void OnEnable()
        {
            NetServer.OnPacketReceived  += AddMessageToChat;
            NetServer.OnPacketSend      += AddMessageToChat;
        }

        private void OnDisable()
        {
            NetServer.OnPacketReceived  -= AddMessageToChat;
            NetServer.OnPacketSend      -= AddMessageToChat;
        }

        private void Awake()
        {
            _server = GetComponent<NetServer>();
        }

        private void Update()
        {
            lock (_mutex)
            {
                foreach (var msg in _chatMessages)
                {
                    OnMessageAdd?.Invoke(msg);
                }

                _chatMessages.Clear();
            }
        }
        private void AddMessageToChat()
        {
            string msg = Encoding.ASCII.GetString(_server.PacketBytes);
            Debug.Log($"Decoding message {msg}");
            lock(_mutex)
            {
                _chatMessages.Add(msg);
            }
        }

        public void SendMessageChat(string message)
        {
            OnMessageAdd?.Invoke(message);
            foreach(var client in _server.ConnectedClients)
            {
                _server.SendPacket(new NetPacketText(message), client);
            }
        }
    }
}