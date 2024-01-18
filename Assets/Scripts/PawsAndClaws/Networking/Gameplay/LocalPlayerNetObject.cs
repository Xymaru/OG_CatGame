using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class LocalPlayerNetObject : DynamicNetworkObject
    {
        [SerializeField] private Transform player;
        private Player.PlayerInputHandler playerInputHandler;
        private Player.PlayerManager playerManager;

        protected override void Start()
        {
            playerInputHandler = player.GetComponent<Player.PlayerInputHandler>();
            playerManager = player.GetComponent<Player.PlayerManager>();

            StartCoroutine(SendPacketCoroutine());
        }

        private void SendPosition()
        {
            NPObjectPos packet = new NPObjectPos();
            packet.net_id = NetID;

            var position = player.position;
            packet.x = position.x;
            packet.y = position.y;

            ReplicationManager.Instance.SendPacket(packet);
        }

        private void SendMovement()
        {
            Vector2 movedir = playerInputHandler.GetDirection();

            NPMoveDirection packet = new NPMoveDirection();
            packet.net_id = NetID;

            packet.dx = movedir.x;
            packet.dy = movedir.y;

            ReplicationManager.Instance.SendPacket(packet);
        }

        private void SendHealth()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Client) return;

            NPPlayerHealth packet = new();
            packet.net_id = NetID;

            packet.health = playerManager.GetHealth();

            ReplicationManager.Instance.SendPacket(packet);
        }

        protected override void SendPackets()
        {
            // Send direction packet
            SendMovement();

            // Send position packet
            SendPosition();

            // Send health packet
            SendHealth();
        }
    }
}
