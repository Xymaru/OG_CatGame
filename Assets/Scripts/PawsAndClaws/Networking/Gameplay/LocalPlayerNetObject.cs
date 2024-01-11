using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class LocalPlayerNetObject : DynamicNetworkObject
    {
        [SerializeField] private Transform player;
        private Player.PlayerInputHandler playerInputHandler;

        protected override void Start()
        {
            playerInputHandler = player.GetComponent<Player.PlayerInputHandler>();

            StartCoroutine(SendPacketCoroutine());
        }

        protected override void SendPackets()
        {
            Vector2 movedir = playerInputHandler.GetDirection();

            NPMoveDirection packet = new NPMoveDirection();
            packet.net_id = NetID;

            packet.dx = movedir.x;
            packet.dy = movedir.y;

            ReplicationManager.Instance.SendPacket(packet);
            
            //NPObjectPos packet = new NPObjectPos();
            //packet.net_id = NetID;

            //var position = player.position;
            //packet.x = position.x;
            //packet.y = position.y;

            //ReplicationManager.Instance.SendPacket(packet);
        }
    }
}
