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
        private readonly MinionManager _minionManager;

        private Coroutine _attackCoroutine;
        public MinionStateAttack(StateMachine stateMachine, GameObject gameObject)
            : base("Minion attack", stateMachine, gameObject)
        {
            _stateMachine = (MinionStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _minionManager = gameObject.GetComponent<MinionManager>();
        }

        public override void Enter()
        {
            _agent.SetDestination(_stateMachine.Target.GameObject.transform.position);
        }
        public override void UpdateLogic()
        {
            _attackCoroutine ??= _stateMachine.StartCoroutine(AttackCoroutine());
        }


        public override void Exit()
        {
            _stateMachine.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
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

        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(_minionManager.timeBetweenAttacks);

            if(_stateMachine.Target is { IsAlive:true })
            {
                _stateMachine.Target.Damage(_minionManager.damage);
            }

            _attackCoroutine = null;
        }
    }
}
