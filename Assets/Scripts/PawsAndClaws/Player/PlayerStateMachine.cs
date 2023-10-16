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
        public Animator animator;

        private Vector3 _target;

        public PlayerIdleState idleState;
        public PlayerMovingState movingState;
        public PlayerAttackState attackState;

        private void Awake()
        {
            idleState = new PlayerIdleState(this, gameObject);
            movingState = new PlayerMovingState(this, gameObject);
            attackState = new PlayerAttackState(this, gameObject);

            var agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            animator = GetComponentInChildren<Animator>();

            inputHandler.InputManager.Gameplay.Move.performed += context => HandlePlayerMoveInput();
        }



        protected override State GetInitialState()
        {
            return idleState;
        }

        void HandlePlayerMoveInput()
        {
            // Check if the player wants to attack
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics2D.Raycast(ray.origin, ray.direction, 1000, GameManager.oppositeTeamLayer))
            {
                movingState.doAttack = true;
                Debug.Log("Player requested attack");
            }
            else
            {
                movingState.doAttack = false;
            }
            
            _target = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            movingState.target = _target;

            ChangeState(movingState);
        }
    }
}
