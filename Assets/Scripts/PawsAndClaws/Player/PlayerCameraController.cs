using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    [RequireComponent(typeof(CameraFollow), typeof(CameraScroll))]
    public class PlayerCameraController : MonoBehaviour
    {

        [SerializeField] private CameraFollow cameraFollowScript;
        [SerializeField] private CameraScroll cameraScrollScript;
        [SerializeField] private bool lockToPlayer = false;
        public PlayerInputHandler inputHandler;
        public Transform player;
        public void Initialize()
        {
            inputHandler.InputManager.Gameplay.LockCamera.performed += context => HandleLockToPlayer();
            cameraFollowScript.player = player;
            cameraScrollScript.enabled = true;
            cameraFollowScript.enabled = false;
        }

        
        void HandleLockToPlayer()
        {
            lockToPlayer = !lockToPlayer;

            if (lockToPlayer)
            {
                cameraScrollScript.enabled = false;
                cameraFollowScript.enabled = true;
            }
            else
            {
                cameraScrollScript.enabled = true;
                cameraFollowScript.enabled = false;
            }
        }
        
       
    }
}
