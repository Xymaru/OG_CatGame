using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class RemotePlayerNetObject : DynamicNetworkObject
    {
        private Player.NetworkPlayerManager networkPlayerManager;

        protected override void Start()
        {
            base.Start();

            networkPlayerManager = GetComponent<Player.NetworkPlayerManager>();
        }

        protected override void SendPackets()
        {
            // Send only position, as move is only through replication
            NPObjectPos packet = new NPObjectPos();
            packet.net_id = NetID;

            var position = transform.position;
            packet.x = position.x;
            packet.y = position.y;

            ReplicationManager.Instance.SendPacket(packet);
        }

        public override void SetPosition(float x, float y)
        {
            if (rb.velocity == Vector2.zero)
            {
                rb.MovePosition(new Vector2(x, y));
            }
        }

        public override void Move(float dx, float dy)
        {
            rb.velocity = new Vector2(dx * networkPlayerManager.CharacterStats.Speed, dy * networkPlayerManager.CharacterStats.Speed);
        }
    }
}