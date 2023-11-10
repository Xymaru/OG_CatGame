using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerIdleState : State
    {
        private PlayerStateMachine _stateMachine;
        private int _speedVar = Animator.StringToHash("Speed");

        public PlayerIdleState(StateMachine stateMachine, GameObject gameObject)
            : base("Player idle", stateMachine, gameObject)
        {
            _stateMachine = (PlayerStateMachine)stateMachine;
           
        }

        public override void Enter()
        {
            _stateMachine.animator.SetFloat(_speedVar, 0f);
        }

        public override void Exit()
        {
        }

        public override void UpdateLogic()
        {
        }
    }
}