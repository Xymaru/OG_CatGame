using System;
using PawsAndClaws.FSM;
using UnityEngine;
using UnityEngine.Serialization;

namespace PawsAndClaws.Entities.Tower
{
    public class TowerStateMachine : StateMachine
    {
        public TowerStateIdle IdleState;
        public TowerStateAttack AttackState;
        public IGameEntity Target = null;
        public SpriteRenderer spriteRenderer;
        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            IdleState = new TowerStateIdle(this, gameObject);
            AttackState = new TowerStateAttack(this, gameObject);
        }

        protected override State GetInitialState()
        {
            return IdleState;
        }
    }
}
