using System;
using System.Collections;
using UnityEngine;

namespace PawsAndClaws.Networking.Gameplay
{
    public class DynamicNetworkObject : NetworkObject
    {
        [SerializeField] private bool isLocalOnly = false;
        private void Start()
        {
            StartCoroutine(SendPacketCoroutine());
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
            var position = transform.position;
            packet.x = position.x;
            packet.y = position.y;
            ReplicationManager.Instance.SendPacket(packet);
        }

        private IEnumerator SendPacketCoroutine()
        {
            while (true)
            {
                SendPackets();
                yield return new WaitForSeconds(NetworkData.PacketSendInterval);
            }
        }
    }
}
