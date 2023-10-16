using PawsAndClaws.Entities;
using PawsAndClaws.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerMovingState : State
    {
        public IGameEntity target;
        public Vector3 moveTarget;

        private PlayerStateMachine _stateMachine;
        public PlayerMovingState(StateMachine stateMachine, GameObject gameObject) 
            : base("Player Idle", stateMachine, gameObject)
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