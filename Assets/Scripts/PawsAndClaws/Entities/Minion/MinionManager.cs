using PawsAndClaws.Player;
using PawsAndClaws.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Entities.Minion
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MinionManager : MonoBehaviour, IGameEntity
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
        bool IGameEntity.IsAlive { get => _isAlive; set { } }

        GameObject IGameEntity.GameObject { get => gameObject; set { } }

        private void Awake()
        {
            gameObject.layer = team == Team.Cat ?
               LayerMask.NameToLayer("Cats") :
               LayerMask.NameToLayer("Hamsters");
        }

        private void Start()
        {
            var collider = GetComponent<CircleCollider2D>();
            collider.radius = range;
            _currentHealth = maxHealth;
            healthBar.UpdateBar(_currentHealth, maxHealth);

            var agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = range - 0.5f;
            agent.speed = speed;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        public void Die()
        {

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

        public void SetMouseAttack()
        {
            if (GameManager.Instance.playerTeam != team)
                PlayerInputHandler.SetCursorAttack();
        }

        public void SetMouseDefault()
        {
            if (GameManager.Instance.playerTeam != team)
                PlayerInputHandler.SetCursorDefault();
        }
    }
}