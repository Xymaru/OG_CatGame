using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerIdleState : State
    {
        private PlayerStateMachine _stateMachine;
        public PlayerIdleState(StateMachine stateMachine, GameObject gameObject)
            : base("Player idle", stateMachine, gameObject)
        {
            _stateMachine = (PlayerStateMachine)stateMachine;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void UpdateLogic()
        {
        }
    }
}