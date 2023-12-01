using System;
using System.Collections;
using PawsAndClaws.Player;
using PawsAndClaws.UI;
using UnityEngine;

namespace PawsAndClaws.Entities.Inhibitor
{
    public class InhibitorController : MonoBehaviour, IGameEntity
    {
        public Team Team { get => team; set {} }
        public bool IsAlive { get => _isAlive; set {} }
        GameObject IGameEntity.GameObject { get => gameObject; set {} }

        [SerializeField] private InGameHealthBarUI healthBar;
        [SerializeField] private Team team;
        [SerializeField] private float maxHealth;
        [SerializeField] private float respawnTime;
        private float _currentHealth;
        private Coroutine _respawnCoroutine;
        private bool _isAlive = true;

        private void Start()
        {
            _currentHealth = maxHealth;
            healthBar.UpdateBar(_currentHealth, maxHealth);
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
            _respawnCoroutine ??= StartCoroutine(RespawnCoroutine());
        }
        private IEnumerator RespawnCoroutine()
        {
            // TODO: Set death animation
            _isAlive = false;
            
            yield return new WaitForSeconds(respawnTime);
            
            // TODO: Respawn the inhibitor
            _currentHealth = maxHealth;
            _isAlive = true;
            _respawnCoroutine = null;
        }

        public void Stun(float stunTime)
        {
            
        }
    }
}
