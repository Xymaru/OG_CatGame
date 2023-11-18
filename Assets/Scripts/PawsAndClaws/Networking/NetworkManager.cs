using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        void Awake()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                gameObject.AddComponent<NetClient>();
            }
            else
            {
                gameObject.AddComponent<NetServerTCP>();
            }
        }

        //public void SetServerInfo()
        //{
        //    lobbyInfoText.text = $"Server \nHosting at IP: {NetworkData.NetSocket.IPAddr.MapToIPv4()} \nConnection mode {NetworkData.ProtocolType.ToString()}";
        //}

        //public void SetClientInfo()
        //{
        //    lobbyInfoText.text = $"Client \nConnected to IP: {NetworkData.NetSocket.Socket.RemoteEndPoint} \nConnection mode {NetworkData.ProtocolType.ToString()}";
        //}
    }
}