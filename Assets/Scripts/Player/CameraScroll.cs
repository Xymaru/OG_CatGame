using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class CameraScroll : MonoBehaviour
    {
        [Header("Camera options")]
        [SerializeField] private float scrollSpeed = 10f;

        [Header("Scroll barriers")]
        [SerializeField] private float topBarrier = 0.97f;        
        [SerializeField] private float bottomBarrier = 0.03f;        
        [SerializeField] private float leftBarrier = 0.03f;        
        [SerializeField] private float rightBarrier = 0.97f;

        private void Update()
        {
            HandleCameraMovementInput();
        }

        void HandleCameraMovementInput()
        {
            // Up and down movement handling
            if(Input.mousePosition.y >= Screen.height * topBarrier)
                transform.Translate(Vector3.up * (Time.deltaTime * scrollSpeed), Space.World);
            else if (Input.mousePosition.y <= Screen.height * bottomBarrier)
                transform.Translate(Vector3.down * (Time.deltaTime * scrollSpeed), Space.World);
            
            // Left and right movement handling
            if(Input.mousePosition.x >= Screen.width * rightBarrier)
                transform.Translate(Vector3.right * (Time.deltaTime * scrollSpeed), Space.World);
            else if (Input.mousePosition.x <= Screen.width * leftBarrier)
                transform.Translate(Vector3.left * (Time.deltaTime * scrollSpeed), Space.World);
        }
    }
}
