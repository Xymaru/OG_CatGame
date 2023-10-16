using PawsAndClaws.Entities;
using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Player
{
    public class PlayerMovingState : State
    {
        public Vector3 target;
        public bool doAttack = false;


        private PlayerStateMachine _stateMachine;
        private NavMeshAgent _agent;
        private PlayerManager _playerManager;
        private SpriteRenderer _spriteRenderer;

        private bool _wasRight = false;
        public PlayerMovingState(StateMachine stateMachine, GameObject gameObject) 
            : base("Player moving", stateMachine, gameObject)
        {
            _stateMachine = (PlayerStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _playerManager = gameObject.GetComponent<PlayerManager>();
            _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        }

        public override void Enter()
        {
            _agent.stoppingDistance = 0;
            if(doAttack)
            {
                _agent.stoppingDistance = _playerManager.CharacterStats.Range;
                Debug.Log("Player moving to then attack");
            }
            _stateMachine.animator.Play("Walk");
        }

        public override void Exit()
        {
        }

        public override void UpdateLogic()
        {
            _agent.SetDestination(target);

            if (_agent.velocity.x >= 0 && _wasRight)
            {
                FlipX();
            }
            else if (_agent.velocity.x < 0 && !_wasRight)
            {
                FlipX();
            }

            if (Vector3.Distance(GameObject.transform.position, target) < 0.5f)
            {
                _stateMachine.ChangeState(doAttack ?
                    _stateMachine.attackState : _stateMachine.idleState);
            }
        }

        private void FlipX()
        {
            _wasRight = !_wasRight;
            _spriteRenderer.flipX = _wasRight;
        }
    }
}