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

        public IGameEntity enemyTarget;

        public PlayerIdleState idleState;
        public PlayerMovingState movingState;
        public PlayerAttackState attackState;

        private LayerMask _oppositeTeam;
        private void Awake()
        {
            idleState = new PlayerIdleState(this, gameObject);
            movingState = new PlayerMovingState(this, gameObject);
            attackState = new PlayerAttackState(this, gameObject);

            var agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            animator = GetComponentInChildren<Animator>();

            inputHandler.InputManager.Gameplay.Move.performed += c => HandlePlayerMoveInput();
            
            _oppositeTeam = Utils.GameUtils.GetOppositeLayer(gameObject);
        }



        protected override State GetInitialState()
        {
            return idleState;
        }

        void HandlePlayerMoveInput()
        {
            // Check if the player wants to attack
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, _oppositeTeam);
            if (hit.collider != null)
            {
                Debug.Log("Player requested attack");
                movingState.doAttack = true;
                var target = Utils.GameUtils.GetIfHasIGameEntity(hit.collider.gameObject);
                if(target is { IsAlive: true })
                {
                    enemyTarget = target;
                }
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
