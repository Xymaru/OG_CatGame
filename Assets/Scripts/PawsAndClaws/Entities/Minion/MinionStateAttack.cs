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
        private readonly MinionController _minionController;

        private Coroutine _attackCoroutine;
        public MinionStateAttack(StateMachine stateMachine, GameObject gameObject)
            : base("Minion attack", stateMachine, gameObject)
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
            _attackCoroutine ??= _stateMachine.StartCoroutine(AttackCoroutine());
            
            if (Vector3.Distance(GameObject.transform.position, _stateMachine.Target.GameObject.transform.position) >
                _minionController.attackRange)
            {
                StateMachine.ChangeState(_stateMachine.ChaseState);
            }
        }


        public override void Exit()
        {
            if (_attackCoroutine != null)
            {
                _stateMachine.StopCoroutine(_attackCoroutine);
            }
            _attackCoroutine = null;
        }
        

        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(_minionController.timeBetweenAttacks + _minionController.timeToAttack);
            
            if(_stateMachine.Target is { IsAlive:true })
            {
                _stateMachine.Target.Damage(_minionController.damage);
            }

            _attackCoroutine = null;
        }
    }
}
