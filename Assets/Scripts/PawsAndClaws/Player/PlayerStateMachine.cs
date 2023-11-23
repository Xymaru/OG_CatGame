using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using PawsAndClaws.FSM;
using PawsAndClaws.Entities;
using PawsAndClaws.Player.States;

namespace PawsAndClaws.Player
{
    public class PlayerStateMachine : StateMachine
    {
        public Animator animator;

        public Vector3 target;
        public IGameEntity EnemyTarget;

        public PlayerIdleState IdleState;
        public PlayerMovingState MovingState;
        public PlayerAttackState AttackState;
        public PlayerChaseState ChaseState;
        
        public override void Start()
        {
            IdleState = new PlayerIdleState(this, gameObject);
            MovingState = new PlayerMovingState(this, gameObject);
            AttackState = new PlayerAttackState(this, gameObject);
            ChaseState = new PlayerChaseState(this, gameObject);

            var agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            animator = GetComponentInChildren<Animator>();
        }
        protected override State GetInitialState()
        {
            return IdleState;
        }
    }
}
