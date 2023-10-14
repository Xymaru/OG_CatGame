using System.Collections;
using PawsAndClaws.FSM;
using PawsAndClaws.Utils;
using UnityEngine;

namespace PawsAndClaws.Entities.Tower
{
    public class TowerStateAttack : State
    {
        private readonly TowerStateMachine _stateMachine;
        private readonly TowerManager _towerManager;
        private Coroutine _attackCoroutine = null;
        public TowerStateAttack(StateMachine stateMachine, GameObject gameObject)
            : base("Tower Attack", stateMachine, gameObject)
        {
            _stateMachine = (TowerStateMachine)stateMachine;
            _towerManager = gameObject.GetComponent<TowerManager>();
        }

        public override void Enter()
        {
            _stateMachine.SpriteRenderer.color = Color.red;
        }

        public override void UpdateLogic()
        {
            if (_stateMachine.Target == null)
            {
                _stateMachine.ChangeState(_stateMachine.IdleState);
                return;
            }

            _attackCoroutine ??= _stateMachine.StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(_towerManager.GetTimeToAttack());
            _towerManager.Attack(_stateMachine.Target);
            _attackCoroutine = null;
        }
        
        public override void Exit()
        {
            _towerManager.ResetTimers();
            _stateMachine.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        public override void OnTriggerExit2D(Collider2D other)
        {
            var gameEntity = GameUtils.GetIfHasIGameEntity(other.gameObject);
            if (gameEntity == null)
                return;
            _stateMachine.Target = null;
        }
    }
}
