using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class MinionNetObject : DynamicNetworkObject
    {
        Entities.Minion.MinionController _minionController = null;

        protected override void Start()
        {
            _minionController = GetComponent<Entities.Minion.MinionController>();

            base.Start();
        }
        protected override void SendPackets()
        {
            NPMinionHealth packet = new();
            packet.net_id = NetID;
            packet.health = _minionController.GetHealth();

            ReplicationManager.Instance.SendPacket(packet);
        }
    }
}