using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace PawsAndClaws.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public Camera playerCamera;
        public PlayerInputHandler inputHandler;
        
        
        private Vector3 _target;
        private NavMeshAgent _agent;
        
        
        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

            inputHandler.InputManager.Gameplay.Move.performed += context => HandlePlayerMoveInput();
        }
        
        void Update()
        {
            UpdatePlayerPosition();
        }

        void UpdatePlayerPosition()
        {
            _agent.SetDestination(new Vector3(_target.x, _target.y, transform.position.z));
        }
        void HandlePlayerMoveInput()
        {
            _target = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
