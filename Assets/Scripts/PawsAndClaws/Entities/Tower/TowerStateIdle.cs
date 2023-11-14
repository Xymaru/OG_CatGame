using PawsAndClaws.FSM;
using PawsAndClaws.Utils;
using UnityEngine;

namespace PawsAndClaws.Entities.Tower
{
    public class TowerStateIdle : State
    {
        private readonly TowerStateMachine _stateMachine;
        public TowerStateIdle(StateMachine stateMachine, GameObject gameObject) 
            : base("Tower Idle", stateMachine, gameObject)
        {
            _stateMachine = (TowerStateMachine)stateMachine;
        }

        public override void Enter()
        {
            _stateMachine.spriteRenderer.color = Color.green;
        }

        public override void UpdateLogic()
        {
            if (_stateMachine.Target is not { IsAlive: true })
                return;
            
            _stateMachine.ChangeState(_stateMachine.AttackState);
        }

        public override void Exit()
        {
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the collision object is eligible
            var gameEntity = GameUtils.GetIfIsEntityFromOtherTeam(GameObject, other.gameObject);
            if (gameEntity is not { IsAlive: true })
                return;
            _stateMachine.Target = gameEntity;
        }
        
    }
}
