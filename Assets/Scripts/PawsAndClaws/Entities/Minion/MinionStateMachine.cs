using PawsAndClaws.FSM;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace PawsAndClaws.Entities.Minion
{
    public class MinionStateMachine : StateMachine
    {
        public Transform CheckPoint;
        public MinionStateMoving MovingState;
        public MinionStateAttack AttackState;
        public IGameEntity Target;


        private void Awake()
        {
            MovingState = new MinionStateMoving(this, gameObject);
            AttackState = new MinionStateAttack(this, gameObject);
        }
        protected override State GetInitialState()
        {
            return MovingState;
        }
    }
}