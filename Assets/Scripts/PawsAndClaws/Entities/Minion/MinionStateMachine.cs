using PawsAndClaws.FSM;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

namespace PawsAndClaws.Entities.Minion
{
    public class MinionStateMachine : StateMachine
    {
        public Transform checkPoint;
        public MinionStateMoving MovingState;
        public MinionStateAttack AttackState;
        public MinionStateChase ChaseState;
        public MinionStateStun StunState;
        public IGameEntity Target;


        private void Awake()
        {
            MovingState = new MinionStateMoving(this, gameObject);
            AttackState = new MinionStateAttack(this, gameObject);
            ChaseState = new MinionStateChase(this, gameObject);
            StunState  = new MinionStateStun(this, gameObject);
        }
        protected override State GetInitialState()
        {
            return MovingState;
        }
    }
}