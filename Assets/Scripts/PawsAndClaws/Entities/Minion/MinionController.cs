using System;
using PawsAndClaws.Player;
using PawsAndClaws.UI;
using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace PawsAndClaws.Entities.Minion
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MinionController : MonoBehaviour, IGameEntity
    {
        [Header("Stats")]
        [SerializeField] private float maxHealth;
        private float _currentHealth;
        [SerializeField] private Team team;
        
        [SerializeField] private float visionRange;
        public float attackRange;
        public float timeToAttack = 1f;
        public float timeBetweenAttacks = 0.5f;
        public float damage = 10f;  
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool wasRight = true;
        
        [SerializeField] private float speed = 2f;

        [Header("References")]
        [SerializeField] private InGameHealthBarUI healthBar;
        [SerializeField] private MinionStateMachine stateMachine;
        
        private NavMeshAgent _agent;
        
        Team IGameEntity.Team { get => team; set => team = value; }
        public bool IsAlive { get => _isAlive; set => _isAlive = value; }
        private bool _isAlive;
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
            circleCollider2D.radius = visionRange;

            _agent = GetComponent<NavMeshAgent>();
            _agent.stoppingDistance = visionRange - 0.5f;
            _agent.speed = speed;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
            
            Initialize();
        }

        public void Update()
        {
            switch (_agent.velocity.x)
            {
                case > 0 when wasRight:
                case < 0 when !wasRight:
                    FlipX();
                    break;
            }
        }

        private void FlipX()
        {
            wasRight = !wasRight;
            spriteRenderer.flipX = wasRight;
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

        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, visionRange);
            Gizmos.DrawWireSphere(transform.position, attackRange);
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
            if (GameManager.Instance.PlayerTeam != team)
                PlayerInputHandler.SetCursorAttack();
        }

        private void SetMouseDefault()
        {
            if (GameManager.Instance.PlayerTeam != team)
                PlayerInputHandler.SetCursorDefault();
        }

        public void Stun(float stunTime)
        {
            // TODO: Stun
            Debug.Log("Enemy stunned");
            stateMachine.StunState.stunTime = stunTime;
            stateMachine.ChangeState(stateMachine.StunState);
        }

        
    }
}