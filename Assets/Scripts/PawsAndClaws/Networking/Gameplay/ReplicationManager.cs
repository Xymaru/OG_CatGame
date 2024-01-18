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

        private List<NPMinionSequence> _minionPackets = new();
        private int _curSequence = 0;
        
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

        public void RegisterNetObject(GameObject obj, int net_id)
        {
            // Resize array to fit net ids
            if (_networkObjects.Count <= net_id)
            {
                int dif = net_id - _networkObjects.Count + 1;

                for (int i = 0; i < dif; i++)
                {
                    _networkObjects.Add(null);
                }
            }

            var netComp = obj.GetComponent<NetworkObject>();
            netComp.NetID = net_id;

            _networkObjects[net_id] = netComp;
        }

        public void SendPacketMinion(NPMinionSequence packet)
        {
            packet.seq = _curSequence;

            _serv.BroadcastPacket(packet);

            _curSequence++;
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

            if (netObj == null) return;

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

        private void ProcessMovePacket(NPMoveDirection p)
        {
            NetworkObject netObj = _networkObjects[p.net_id];

            if (netObj == null) return;

            DynamicNetworkObject dyn_obj = netObj as DynamicNetworkObject;

            dyn_obj.Move(p.dx, p.dy);

            if (NetworkData.NetSocket.NetCon == NetCon.Host)
            {
                _serv.BroadcastPacket(p);
            }
        }

        private void CheckNextMinionPacket()
        {
            foreach(NPMinionSequence p in _minionPackets)
            {
                if(p.seq == _curSequence)
                {
                    ProcessPacket(p);
                    break;
                }
            }
        }

        private void ProcessMinionSpawn(NPMinionSpawn p)
        {
            if (p.seq == _curSequence)
            {
                Game.GameManager.Instance.SpawnMinion(p.team);
                _curSequence++;

                CheckNextMinionPacket();
            }
            else
            {
                _minionPackets.Add(p);
            }
        }

        private void ProcessMinionDeath(NPMinionDeath p)
        {
            if (p.seq == _curSequence)
            {
                MinionNetObject netObj = _networkObjects[p.net_id] as MinionNetObject;

                netObj.GetComponent<Entities.Minion.MinionController>().Die();

                _curSequence++;

                CheckNextMinionPacket();
            }
            else
            {
                _minionPackets.Add(p);
            }
        }

        private void ProcessMinionHealth(NPMinionHealth p)
        {
            MinionNetObject netObj = _networkObjects[p.net_id] as MinionNetObject;

            netObj.GetComponent<Entities.Minion.MinionController>().SetHealth(p.health);
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
                case NPacketType.MOVEDIR:
                    ProcessMovePacket(packet as NPMoveDirection);
                    break;
                case NPacketType.MINIONSPAWN:
                    ProcessMinionSpawn(packet as NPMinionSpawn);
                    break;
                case NPacketType.MINIONDEATH:
                    ProcessMinionDeath(packet as NPMinionDeath);
                    break;
                case NPacketType.MINIONHEALTH:
                    ProcessMinionHealth(packet as NPMinionHealth);
                    break;
                default:
                    break;
            }
        }
    }
}