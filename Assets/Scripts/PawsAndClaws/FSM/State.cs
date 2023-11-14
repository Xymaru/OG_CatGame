using PawsAndClaws.Entities;
using UnityEngine;

namespace PawsAndClaws.FSM
{
    public abstract class State
    {
        public readonly string Name;
        protected readonly StateMachine StateMachine;
        protected readonly GameObject GameObject;

        protected State(string name, StateMachine stateMachine, GameObject gameObject)
        {
            this.Name = name;
            StateMachine = stateMachine;
            GameObject = gameObject;
        }

        public abstract void Enter();
        public abstract void UpdateLogic();
        public abstract void Exit();

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
        }

        public virtual void OnTriggerStay2D(Collider2D other)
        {
            
        }
        public virtual void OnTriggerExit2D(Collider2D other)
        {
        }
        
        
    }
}