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

        private void OnEnable()
        {
            NetChatClient.OnMessageReceived += AddChatMessage;
            NetChatServer.OnMessageReceived += AddChatMessage;
        }

        private void OnDisable()
        {
            NetChatClient.OnMessageReceived -= AddChatMessage;
            NetChatServer.OnMessageReceived -= AddChatMessage;
        }

        private void Awake()
        {
            sendButton.onClick.AddListener(SendMessage);
        }

        private void SendMessage()
        {
            
        }

        private void AddChatMessage(string msg)
        {
            chatBox.text += $"{msg} \n";
        }
    }
}
