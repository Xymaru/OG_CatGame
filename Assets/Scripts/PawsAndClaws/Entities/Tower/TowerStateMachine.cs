using System;
using PawsAndClaws.FSM;
using UnityEngine;

namespace PawsAndClaws.Entities.Tower
{
    public class TowerStateMachine : StateMachine
    {
        public TowerStateIdle IdleState;
        public TowerStateAttack AttackState;
        public IGameEntity Target = null;
        public SpriteRenderer SpriteRenderer;
        private void Awake()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            IdleState = new TowerStateIdle(this, gameObject);
            AttackState = new TowerStateAttack(this, gameObject);
        }

        protected override State GetInitialState()
        {
            return IdleState;
        }
    }
}
