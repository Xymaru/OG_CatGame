using System;
using PawsAndClaws.Player;
using PawsAndClaws.UI;
using UnityEngine;

namespace PawsAndClaws.Entities.Tower
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TowerManager : MonoBehaviour, IGameEntity
    {
        [Header("Tower stats")]
        [SerializeField] private float range = 10f;
        [SerializeField] private float maxHealth = 1000f;
        private float _currentHealth;
        public float timeToAttack = 1f;
        public float timeBetweenAttacks = 0.5f;
        public int timesAttacked = 0;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float damageMultiplier = 1.4f;
        [SerializeField] private Team team;

        [Header("References")]
        [SerializeField] private InGameHealthBarUI healthBar;

        private bool _isAlive;

        Team IGameEntity.Team
        {
            get => team;
            set { }
        }

        bool IGameEntity.IsAlive
        {
            get => _isAlive;
            set {}
        }

        private void Awake()
        {
            _currentHealth = maxHealth;
            var collider = GetComponent<CircleCollider2D>();
            collider.radius = range;
        }

        private void Start()
        {
            healthBar.UpdateBar(_currentHealth, maxHealth);
        }

        public void Attack(IGameEntity target)
        {
            if (target == null)
                return;
            timesAttacked++;
            var totalDamage = damage + (timesAttacked * damageMultiplier);
            
            if (target.Damage(totalDamage))
                target = null;
        }

        public void ResetTimers()
        {
            timesAttacked = 0;
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

        public void Die()
        {
            
        }

        public float GetTimeToAttack()
        {
            return timesAttacked > 0 ? timeBetweenAttacks : timeToAttack;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }


        private void OnMouseOver()
        {
            if(GameManager.Instance.playerTeam != team)
                PlayerInputHandler.SetCursorAttack();
        }

        private void OnMouseExit()
        {
            if(GameManager.Instance.playerTeam != team)
                PlayerInputHandler.SetCursorDefault();
        }
    }
}
