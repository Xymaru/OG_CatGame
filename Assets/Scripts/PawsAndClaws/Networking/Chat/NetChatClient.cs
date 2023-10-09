using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class NetChatClient : MonoBehaviour
    {
        public static Action<string> OnMessageReceived;
        private NetClient _client;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Awake()
        {
            _client = GetComponent<NetClient>();
        }
    }
}