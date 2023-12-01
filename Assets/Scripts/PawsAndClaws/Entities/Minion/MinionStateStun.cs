using PawsAndClaws.Entities.Minion;
using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Entities
{
    public class MinionStateStun : State
    {
        public float stunTime = 0f;
        private float _timer = 0f;
        private MinionStateMachine _stateMachine;
        public MinionStateStun(StateMachine stateMachine, GameObject gameObject)
            : base("Minion stun", stateMachine, gameObject)
        {
            _stateMachine = (MinionStateMachine)stateMachine;
        }

        public override void Enter()
        {
            _timer = stunTime;
        }

        public override void Exit()
        {
            
        }

        public override void UpdateLogic()
        {
            _timer -= Time.deltaTime;
            if(_timer < 0f ) 
            {
                _stateMachine.ChangeState(_stateMachine.MovingState);
            }
        }
    }
}