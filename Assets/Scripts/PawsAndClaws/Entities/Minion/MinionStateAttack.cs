using PawsAndClaws.FSM;
using PawsAndClaws.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Entities.Minion
{
    public class MinionStateAttack : State
    {
        private readonly MinionStateMachine _stateMachine;
        private readonly NavMeshAgent _agent;
        
        public MinionStateAttack(StateMachine stateMachine, GameObject gameObject)
            : base("Minion attack", stateMachine, gameObject)
        {
            _stateMachine = (MinionStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void Enter()
        {
        }
        public override void UpdateLogic()
        {
        }
        public override void Exit()
        {
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            // Check if the collision object is eligible
            var gameEntity = GameUtils.GetIfIsEntityFromOtherTeam(GameObject, other.gameObject);
            if (gameEntity == _stateMachine.Target)
            {
                _stateMachine.Target = null;
                _stateMachine.ChangeState(_stateMachine.MovingState);
            }
        }
    }
}
