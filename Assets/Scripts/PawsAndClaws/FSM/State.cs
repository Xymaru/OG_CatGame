using PawsAndClaws.Entities;
using UnityEngine;

namespace PawsAndClaws.FSM
{
    public abstract class State
    {
        public string Name;
        protected StateMachine StateMachine;
        protected GameObject GameObject;

        public State(string name, StateMachine stateMachine, GameObject gameObject)
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

        public virtual void OnTriggerExit2D(Collider2D other)
        {
        }
        
        
    }
}