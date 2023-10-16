using System;
using System.Collections;
using UnityEngine;

namespace PawsAndClaws.FSM
{
    public class StateMachine : MonoBehaviour
    {
        protected State CurrentState;
        private void Start()
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
            CurrentState?.Exit();
            Debug.Log($"Exiting {CurrentState?.Name} and entering {newState.Name}");
            CurrentState = newState;
            CurrentState.Enter();
        }
        
        public void StopAll()
        {
            StopAllCoroutines();
        }
        public Coroutine Execute(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void Stop(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        
        protected virtual State GetInitialState()
        {
            return null;
        }

        protected void OnTriggerEnter2D(Collider2D other)
        {
            CurrentState?.OnTriggerEnter2D(other);
        }

        protected void OnTriggerExit2D(Collider2D other)
        {
            CurrentState?.OnTriggerExit2D(other);
        }
    }
}
