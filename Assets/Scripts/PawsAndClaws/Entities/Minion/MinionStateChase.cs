using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.FSM;
using PawsAndClaws.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Entities.Minion
{
    public class MinionStateChase : State
    {
        private readonly MinionStateMachine _stateMachine;
        private readonly NavMeshAgent _agent;

        public MinionStateChase(StateMachine stateMachine, GameObject gameObject)
            : base("Minion chase", stateMachine, gameObject)
        {
            _stateMachine = (MinionStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
            _agent.SetDestination(_stateMachine.Target.GameObject.transform.position);
        }

        public override void UpdateLogic()
        {

        }

        public override void Exit()
        {
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            CheckIfOutOfRange(other);
        }

        private void CheckIfOutOfRange(Collider2D other)
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
            CheckIfOutOfRange(other);
        }
    }
}
