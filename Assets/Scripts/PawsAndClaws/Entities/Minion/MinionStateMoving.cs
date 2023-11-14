using PawsAndClaws.Entities.Tower;
using PawsAndClaws.FSM;
using PawsAndClaws.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Entities.Minion
{
    public class MinionStateMoving : State
    {
        private readonly MinionStateMachine _stateMachine;
        private readonly NavMeshAgent _agent;

        public MinionStateMoving(StateMachine stateMachine, GameObject gameObject)
            : base("Minion moving", stateMachine, gameObject)
        {
            _stateMachine = (MinionStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
            var target = _stateMachine.Target == null ? _stateMachine.CheckPoint : _stateMachine.Target.GameObject.transform;
            _agent.SetDestination(target.position);
        }

        public override void UpdateLogic()
        {

        }

        public override void Exit()
        {
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            CheckIfCanAttack(other);
        }

        private void CheckIfCanAttack(Collider2D other)
        {
            // Check if the collision object is eligible
            var gameEntity = GameUtils.GetIfIsEntityFromOtherTeam(GameObject, other.gameObject);
            if (gameEntity is not { IsAlive: true })
                return;
            _stateMachine.Target = gameEntity;
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }

        public override void OnTriggerStay2D(Collider2D other)
        {
            CheckIfCanAttack(other);
        }
    }
}
