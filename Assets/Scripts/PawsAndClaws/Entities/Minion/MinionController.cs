using System;
using PawsAndClaws.Player;
using PawsAndClaws.UI;
using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Entities.Minion
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MinionController : MonoBehaviour, IGameEntity
    {
        [Header("Stats")]
        [SerializeField] private float maxHealth;
        private float _currentHealth;
        [SerializeField] private float range;
        public float timeToAttack = 1f;
        public float timeBetweenAttacks = 0.5f;
        [SerializeField] private float speed = 2f;
        public float damage = 10f;  

        [SerializeField] private Team team;

        [Header("References")]
        [SerializeField] private InGameHealthBarUI healthBar;
        
        private bool _isAlive;
        Team IGameEntity.Team { get => team; set => team = value; }
        public bool IsAlive { get => _isAlive; set => _isAlive = value; }
        GameObject IGameEntity.GameObject { get => gameObject; set { } }

        private void Awake()
        {
            gameObject.layer = team == Team.Cat ?
                GameConstants.CatLayerMask:
                GameConstants.HamsterLayerMask;
        }

        public void Start()
        {
            var circleCollider2D = GetComponent<CircleCollider2D>();
            circleCollider2D.radius = range;

            var agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = range - 0.5f;
            agent.speed = speed;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            
            Initialize();
        }

        public void Initialize()
        {
            _currentHealth = maxHealth;
            healthBar.UpdateBar(_currentHealth, maxHealth);

            var stateMachine = GetComponent<MinionStateMachine>();
            stateMachine.Start();
        }

        public void Die()
        {
            _isAlive = false;
            gameObject.SetActive(false);
        }

        public bool Damage(float incomingDamage)
        {
            _currentHealth -= incomingDamage;
            healthBar.UpdateBar(_currentHealth, maxHealth);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
                return true;
            }
            return false;
        }

        


        protected void OnMouseOver()
        {
            SetMouseAttack();
        }

        protected void OnMouseExit()
        {
            SetMouseDefault();
        }

        private void SetMouseAttack()
        {
            if (GameManager.Instance.playerTeam != team)
                PlayerInputHandler.SetCursorAttack();
        }

        private void SetMouseDefault()
        {
            if (GameManager.Instance.playerTeam != team)
                PlayerInputHandler.SetCursorDefault();
        }
    }
}