using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking {
    public class NetworkGameplayManager : MonoBehaviour
    {
        NetClientUDP _client;
        NetServerUDP _serv;
        void Start()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                _client = gameObject.AddComponent<NetClientUDP>();
            }
            else
            {
                _serv = gameObject.AddComponent<NetServerUDP>();
            }
        }

        void Update()
        {

        }

        public void SendPosition()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                _client?.SendPlayerPos();
            }
            else
            {
                _serv?.SendPlayerPos();
            }
        }
    }
}