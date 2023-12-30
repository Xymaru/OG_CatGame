using System;
using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Networking.Gameplay;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class ReplicationManager : MonoBehaviour
    {
        public static ReplicationManager Instance { get; private set; }
        private readonly List<NetworkObject> _networkObjects = new List<NetworkObject>();
        
        private NetClientUDP _client;
        private NetServerUDP _serv;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        private void Start()
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

        public GameObject CreateNetObject(GameObject prefab, Vector3 position)
        {
            var id = _networkObjects.Count;
            var netObject = Instantiate(prefab, position, Quaternion.identity);
            var netComp = netObject.GetComponent<NetworkObject>();
            netComp.NetID = id;
            _networkObjects.Add(netComp);

            return netObject;
        }

        public GameObject CreateNetObject(GameObject prefab, Vector3 position, int net_id)
        {
            // Resize array to fit net ids
            if(_networkObjects.Count <= net_id)
            {
                int dif = net_id - _networkObjects.Count + 1;

                for(int i = 0; i < dif; i++)
                {
                    _networkObjects.Add(null);
                }
            }

            var netObject = Instantiate(prefab, position, Quaternion.identity);
            var netComp = netObject.GetComponent<NetworkObject>();
            netComp.NetID = net_id;

            _networkObjects[net_id] = netComp;

            return netObject;
        }

        public void SendPacket(NetworkPacket packet)
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client)
            {
                _client.SendPacket(packet);
            }
            else
            {
                _serv.BroadcastPacket(packet);
            }
        }

        private void ProcessPosPacket(NPObjectPos p)
        {
            DynamicNetworkObject netObj = _networkObjects[p.net_id] as DynamicNetworkObject;
            if (netObj != null)
                netObj.SetPosition(p.x, p.y);
        }

        private void ProcessAbilityPacket(NPAbility p)
        {
            NetworkObject netObj = _networkObjects[p.net_id];

            if (netObj == null) return;

            Player.NetworkPlayerManager netPlayer = netObj.GetComponentInChildren<Player.NetworkPlayerManager>();

            if (netPlayer == null) return;

            netPlayer.ActivateAbility(p.ab_number, p);

            if (NetworkData.NetSocket.NetCon == NetCon.Host)
            {
                _serv.BroadcastPacket(p);
            }
        }

        public void ProcessPacket(NetworkPacket packet)
        {
            switch (packet.p_type)
            {
                case NPacketType.OBJECTPOS:
                    ProcessPosPacket(packet as NPObjectPos);
                    break;
                case NPacketType.ABILITY:
                    ProcessAbilityPacket(packet as NPAbility);
                    break;
                default:
                    break;
            }
        }
    }
}