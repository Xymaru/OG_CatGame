using System;
using System.Collections;
using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class DynamicNetworkObject : NetworkObject
    {
        [SerializeField] private bool isLocalOnly = false;
        protected virtual void Start()
        {
            if (NetworkData.NetSocket.NetCon == NetCon.Host)
            {
                StartCoroutine(SendPacketCoroutine());
            }
        }

        public void SetPosition(float x, float y)
        {
            if (isLocalOnly)
                return;
            transform.position = new Vector3(x, y, transform.position.z);
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
