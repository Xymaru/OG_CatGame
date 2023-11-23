using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;


namespace PawsAndClaws.Player
{
    public class PlayerAttackState : State
    {
        private PlayerStateMachine _stateMachine;
        private Coroutine _attackCoroutine;
        private PlayerManager _playerManager;

        public PlayerAttackState(StateMachine stateMachine, GameObject gameObject)
            : base("Player attack", stateMachine, gameObject)
        {
            _stateMachine = (PlayerStateMachine)stateMachine;
            _playerManager = gameObject.GetComponent<PlayerManager>();
        }

        public override void Enter()
        {
        }

        public override void UpdateLogic()
        {
            _attackCoroutine ??= StateMachine.StartCoroutine(AttackCoroutine());
        }
        private IEnumerator AttackCoroutine()
        {
            _stateMachine.animator.SetTrigger(GameConstants.AutoAttackAnim);
            _playerManager.Attack(_stateMachine.EnemyTarget);

            yield return new WaitForSeconds(1f / _playerManager.CharacterStats.TotalAttackSpeed);

            _attackCoroutine = null;
        }
        public override void Exit()
        {
            if(_attackCoroutine != null)
            {
                StateMachine.StopCoroutine(_attackCoroutine);
            }
        }
    }
}