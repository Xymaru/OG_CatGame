using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetChatServer : MonoBehaviour
    {
        public static Action<string> OnMessageReceived;

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
            foreach(var msg in _chatMessages)
            {
                OnMessageReceived?.Invoke(msg);
            }

            _chatMessages.Clear();
        }

        private void AddMessageToChat()
        {
            lock(_mutex)
            {
                string msg = Utils.BinaryUtils.ByteArrayToObject<string>(_server.PacketBytes);
                _chatMessages.Add(msg);
            }
        }
    }
}