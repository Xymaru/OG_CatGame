using System;
using System.Collections;
using UnityEngine;

namespace PawsAndClaws.FSM
{
    public class StateMachine : MonoBehaviour
    {
        protected State CurrentState;
        public virtual void Start()
        {
            CurrentState = GetInitialState();
            CurrentState?.Enter();
        }

        private void Update()
        {
            CurrentState?.UpdateLogic();
        }

        public void ChangeState(State newState)
        {
            if(CurrentState == newState)
                return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        public void StopAll() => StopAllCoroutines();
        public Coroutine Execute(IEnumerator routine) => StartCoroutine(routine);
        public void Stop(IEnumerator routine) => StopCoroutine(routine);
        public string GetCurrentStateName() => CurrentState.Name;
        protected virtual State GetInitialState() => null;
        protected void OnTriggerEnter2D(Collider2D other) => CurrentState?.OnTriggerEnter2D(other);
        protected void OnTriggerExit2D(Collider2D other) => CurrentState?.OnTriggerExit2D(other);

        protected void OnTriggerStay2D(Collider2D other) => CurrentState?.OnTriggerStay2D(other);
    }
}
