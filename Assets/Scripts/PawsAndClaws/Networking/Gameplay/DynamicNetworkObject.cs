using System;
using System.Collections;
using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class DynamicNetworkObject : NetworkObject
    {
        [SerializeField] private bool isLocalOnly = false;

        protected Rigidbody2D rb;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            if (NetworkData.NetSocket.NetCon == NetCon.Host)
            {
                StartCoroutine(SendPacketCoroutine());
            }
        }

        public virtual void SetPosition(float x, float y)
        {
            if (isLocalOnly)
                return;

            if (rb)
            {
                rb.MovePosition(new Vector2(x, y));
            }
            else
            {
                transform.position = new Vector3(x, y, transform.position.z);
            }
        }

        public virtual void Move(float dx, float dy)
        {
            if (isLocalOnly)
                return;

            rb.velocity = new Vector2(dx, dy);
        }

        protected virtual void SendPackets()
        {
            NPObjectPos packet = new NPObjectPos();
            packet.net_id = NetID;

            var position = transform.position;
            packet.x = position.x;
            packet.y = position.y;

            ReplicationManager.Instance.SendPacket(packet);
        }

        protected IEnumerator SendPacketCoroutine()
        {
            while (true)
            {
                SendPackets();
                yield return new WaitForSeconds(NetworkData.PacketSendInterval);
            }
        }
    }
}
