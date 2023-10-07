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
            NetServerTCP.OnMessageReceived += AddChatMessage;
            NetServerUDP.OnMessageReceived += AddChatMessage;
        }

        private void OnDisable()
        {
            NetServerTCP.OnMessageReceived -= AddChatMessage;
            NetServerUDP.OnMessageReceived -= AddChatMessage;
        }

        private void Awake()
        {
            sendButton.onClick.AddListener(SendMessage);
        }

        private void SendMessage()
        {
            byte[] buff = Encoding.ASCII.GetBytes(inputField.text);
            if(NetworkData.ProtocolType == ProtocolType.Tcp)
                NetworkData.NetSocket.Socket.Send(buff);
        }

        private void AddChatMessage(string msg)
        {
            chatBox.text += $"{msg} \n";
        }
    }
}
