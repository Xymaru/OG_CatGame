using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
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
            _stateMachine.animator.Play(GameConstants.AutoAttackAnim);
            yield return new WaitForSeconds(1 / _playerManager.CharacterStats.TotalAttackSpeed);

            _playerManager.Attack(_stateMachine.enemyTarget);

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