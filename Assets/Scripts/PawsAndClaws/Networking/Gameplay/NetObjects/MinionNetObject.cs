using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class MinionNetObject : DynamicNetworkObject
    {
        protected override void SendPackets()
        {
            //NPObjectPos packet = new NPObjectPos();
            //packet.net_id = NetID;

            //var position = transform.position;
            //packet.x = position.x;
            //packet.y = position.y;

            //ReplicationManager.Instance.SendPacket(packet);
        }
    }
}