using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetChatClient : MonoBehaviour, INetChat
    {
        public static Action<string> OnMessageAdd;
        private NetClient _client;

        private List<string> _chatMessages = new List<string>();
        private object _mutex = new object();
        private void Awake()
        {
            _client = GetComponent<NetClient>();
            _client.onPacketReceived += HandleMessageReceived;
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

        private void OnDestroy()
        {
            _client.onPacketReceived -= HandleMessageReceived;
        }

        public void SendMessageChat(string message)
        {
            _client.SendPacket(new NetPacketText(message));
            OnMessageAdd?.Invoke(message);
        }

        public void HandleMessageReceived()
        {
            NetPacketText packetText = Utils.BinaryUtils.ByteArrayToObject<NetPacketText>(_client.PacketBytes);
            Debug.Log($"Client decoded message {packetText.text}");
            lock(_mutex)
            {
                _chatMessages.Add(packetText.text);
            }
        }
    }
}