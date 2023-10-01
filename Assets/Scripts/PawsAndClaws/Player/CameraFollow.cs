using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform player;
        private Vector3 _cameraOffset = new Vector3(0f, 0f, -10f);

        [Range(0.01f, 1.0f)] public float smoothness = 0.5f;
        
        private void Update()
        {
            FollowPlayer();
        }

        void FollowPlayer()
        {
            Vector3 newPos = player.position + _cameraOffset;
            transform.position = Vector3.Slerp(transform.position, newPos, smoothness);
        }
    }
}
