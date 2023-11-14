using System;
using PawsAndClaws.Game;
using PawsAndClaws.Player;
using PawsAndClaws.UI;
using UnityEngine;

namespace PawsAndClaws.Entities.Tower
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TowerController : MonoBehaviour, IGameEntity
    {
        [Header("Tower stats")]
        [SerializeField] private float range = 10f;
        [SerializeField] private float maxHealth = 1000f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float damageMultiplier = 1.4f;
        [SerializeField] private Team team;

        [Header("References")]
        [SerializeField] private InGameHealthBarUI healthBar;

        private float _currentHealth;
        public float timeToAttack = 1f;
        public float timeBetweenAttacks = 0.5f;
        public int timesAttacked = 0;
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

        GameObject IGameEntity.GameObject { get => gameObject; set { } }

        private void Awake()
        {
            gameObject.layer = team == Team.Cat ?
                GameConstants.CatLayerMask:
                GameConstants.HamsterLayerMask;
        }

        private void Start()
        {
            var collider = GetComponent<CircleCollider2D>();
            collider.radius = range;
            _currentHealth = maxHealth;
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

        bool IGameEntity.Damage(float damage)
        {
            return true;
        }

        void IGameEntity.Die()
        {
        }
    }
}
