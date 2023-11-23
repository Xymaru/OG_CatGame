using PawsAndClaws.Entities.Minion;
using PawsAndClaws.FSM;
using PawsAndClaws.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Player.States
{
    public class PlayerChaseState : State
    {
        private readonly PlayerStateMachine _stateMachine;
        private readonly NavMeshAgent _agent;
        private readonly PlayerManager _playerController;

        public PlayerChaseState(StateMachine stateMachine, GameObject gameObject)
            : base("Player chase", stateMachine, gameObject)
        {
            _stateMachine = (PlayerStateMachine)stateMachine;
            _agent = gameObject.GetComponent<NavMeshAgent>();
            _playerController = gameObject.GetComponent<PlayerManager>();
        }

        public override void Enter()
        {
            _agent.SetDestination(_stateMachine.EnemyTarget.GameObject.transform.position);
            _agent.stoppingDistance = _playerController.CharacterStats.Range;
        }

        public override void UpdateLogic()
        {
            if (Vector3.Distance(GameObject.transform.position, _stateMachine.EnemyTarget.GameObject.transform.position) <
                _playerController.CharacterStats.Range)
            {
                StateMachine.ChangeState(_stateMachine.AttackState);
            }
        }

        public override void Exit()
        {
        }
        
    }
}
