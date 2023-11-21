using PawsAndClaws.Entities;
using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
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

            
        }

        public override void Exit()
        {
        }

        public override void UpdateLogic()
        {
            _agent.SetDestination(target);
            _stateMachine.animator.SetFloat(GameConstants.SpeedAnim, _agent.velocity.magnitude);

            switch (_agent.velocity.x)
            {
                case > 0 when _wasRight:
                case < 0 when !_wasRight:
                    FlipX();
                    break;
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