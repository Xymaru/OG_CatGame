using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using PawsAndClaws.FSM;
using PawsAndClaws.Entities;

namespace PawsAndClaws.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public Animator animator;

        public Vector3 target;

        public IGameEntity enemyTarget;

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
        }
        protected override State GetInitialState()
        {
            return idleState;
        }
    }
}
