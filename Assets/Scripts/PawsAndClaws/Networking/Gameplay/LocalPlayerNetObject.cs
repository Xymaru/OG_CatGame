using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class LocalPlayerNetObject : DynamicNetworkObject
    {
        [SerializeField] private Transform player;

        protected override void SendPackets()
        {
            NPObjectPos packet = new NPObjectPos();
            var position = player.position;
            packet.x = position.x;
            packet.y = position.y;
            ReplicationManager.Instance.SendPacket(packet);
        }
    }
}
