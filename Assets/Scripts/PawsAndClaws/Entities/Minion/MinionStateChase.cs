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
        private readonly MinionController _minionController;

        public MinionStateChase(StateMachine stateMachine, GameObject gameObject)
            : base("Minion chase", stateMachine, gameObject)
        {
            _stateMachine = (MinionStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _minionController = gameObject.GetComponent<MinionController>();
        }

        public override void Enter()
        {
            _agent.SetDestination(_stateMachine.Target.GameObject.transform.position);
        }

        public override void UpdateLogic()
        {
            if (Vector3.Distance(GameObject.transform.position, _stateMachine.Target.GameObject.transform.position) <
                _minionController.attackRange)
            {
                StateMachine.ChangeState(_stateMachine.AttackState);
            }
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
            if (gameEntity != null)
            {
                _stateMachine.ChangeState(_stateMachine.MovingState);
            }
        }

        
    }
}
