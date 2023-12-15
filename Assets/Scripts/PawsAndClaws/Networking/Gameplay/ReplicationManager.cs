using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Networking
{
    public class ReplicationManager : MonoBehaviour
    {
        public static ReplicationManager Instance { get; private set; }
        private List<NetworkObject> networkObjects = new List<NetworkObject>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }

        public GameObject CreateNetObject(GameObject prefab, Vector3 position)
        {
            var id = networkObjects.Count;
            var netObject = Instantiate(prefab, position, Quaternion.identity);
            var netComp = netObject.AddComponent<NetworkObject>();
            netComp.NetID = id;

            networkObjects.Add(netComp);

            return netObject;
        }


        public void ProcessPacket(NetworkPacket packet)
        {

        }
    }
}