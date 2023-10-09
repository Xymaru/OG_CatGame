using PawsAndClaws.Networking;
using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class NetChatUI : MonoBehaviour
    {
        [SerializeField] private Button sendButton;
        [SerializeField] private TMPro.TMP_InputField inputField;
        [SerializeField] private TMPro.TextMeshProUGUI chatBox;
        [SerializeField] private NetworkManager manager;
        private INetChat _chat;

        private void OnEnable()
        {
            NetChatClient.OnMessageAdd += AddChatMessage;
            NetChatServer.OnMessageAdd += AddChatMessage;
        }

        private void OnDisable()
        {
            NetChatClient.OnMessageAdd -= AddChatMessage;
            NetChatServer.OnMessageAdd -= AddChatMessage;
        }

        private void Start()
        {
            _chat = manager.GetComponent<INetChat>();
            sendButton.onClick.AddListener(SendMessage);
        }

        private void SendMessage()
        {
            _chat.SendMessageChat(inputField.text);
            Debug.Log($"Sending message {inputField.text}");
        }

        private void AddChatMessage(string msg)
        {
            chatBox.text += $"{msg}\n";
        }
    }
}
