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
            
            var go = gameObject;
            IdleState = new TowerStateIdle(this, go);
            AttackState = new TowerStateAttack(this, go);
        }

        protected override State GetInitialState()
        {
            return IdleState;
        }
    }
}
