using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using PawsAndClaws.FSM;
using PawsAndClaws.Entities;

namespace PawsAndClaws.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public Camera playerCamera;
        public PlayerInputHandler inputHandler;
        
        private Vector3 _movementTarget;
        private IGameEntity _target;
        private NavMeshAgent _agent;

        public PlayerIdleState idleState;
        public PlayerMovingState movingState;

        private void Awake()
        {
            idleState = new PlayerIdleState(this, gameObject);
            movingState = new PlayerMovingState(this, gameObject);
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

            inputHandler.InputManager.Gameplay.Move.performed += context => HandlePlayerMoveInput();
        }


        protected override State GetInitialState()
        {
            return idleState;
        }

        void HandlePlayerMoveInput()
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, GameManager.oppositeTeamLayer))
            {
                _target = Utils.GameUtils.GetIfHasIGameEntity(hit.collider.gameObject);   
            }

            _movementTarget = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            ChangeState(movingState);
        }
    }
}
