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
            // Player packets done only through replication manager in host
        }

        public override void Move(float dx, float dy)
        {
            rb.velocity = new Vector2(dx * networkPlayerManager.CharacterStats.Speed, dy * networkPlayerManager.CharacterStats.Speed);
        }
    }
}