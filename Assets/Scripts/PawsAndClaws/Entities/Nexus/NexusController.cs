using System;
using PawsAndClaws.Game;
using PawsAndClaws.Player;
using PawsAndClaws.UI;
using UnityEngine;

namespace PawsAndClaws.Entities.Nexus
{
    public class NexusController : MonoBehaviour, IGameEntity
    {
        public Team Team { get => team; set { } }
        public bool IsAlive { get => _isAlive; set {} }
        GameObject IGameEntity.GameObject { get => gameObject; set {} }
        
        [SerializeField] private InGameHealthBarUI healthBar;
        [SerializeField] private Team team;
        [SerializeField] private float maxHealth;
        private float _currentHealth;
        
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
            GameManager.Instance.LoseGame(team);
        }

        public void Stun(float stunTime)
        {
            
        }
    }
}
