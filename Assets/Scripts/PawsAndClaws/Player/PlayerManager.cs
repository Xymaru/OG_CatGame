using PawsAndClaws.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PawsAndClaws.Entities;
using PawsAndClaws.Game;
using PawsAndClaws.Player.Abilities;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Player
{
    public class PlayerManager : MonoBehaviour, IGameEntity
    {
        [SerializeField] protected List<Ability> abilities = new List<Ability>();
        
        // Character data
        public CharacterDataSO characterData;
        public CharacterStats CharacterStats;
        protected GameObject _character;
        public string userName;
        
        // Events
        public Action<float, float> onHealthChange;
        public Action<float> onHealthRegenChange;
        public Action<float, float> onManaChange;
        public Action<float> onManaRegenChange;
        public Action<float, float> onExpChange;
        public Action<int> onLevelUp;
        public Action<float> onPlayerDied;
        public Action onPlayerSpawn;
        public Action<CharacterStats> onStatsChanged;
        public Action onAutoAttack;
        
        // Respawn variables
        public Transform spawnPoint;
        protected const float BaseRespawnTime = 30f;
        protected const float LevelRespawnMultiplier = 2f;
        protected Coroutine _respawnCoroutine;
       
        
        protected InGamePlayerHealthBarUI _healthBar;
        protected Rigidbody2D rigidBody;
        public Team Team {get => characterData.team; set { } }
        public bool IsAlive { get => _isAlive; set { } }
        protected bool _isAlive = true;
        GameObject IGameEntity.GameObject { get => gameObject; set {} }

        private Networking.Gameplay.DynamicNetworkObject _playerNetObj;

        protected virtual void Start()
        {
            _playerNetObj = GetComponent<Networking.Gameplay.DynamicNetworkObject>();

            if(_playerNetObj == null)
            {
                _playerNetObj = GetComponentInParent<Networking.Gameplay.DynamicNetworkObject>();
            }
        }

        protected void CollectAbilities()
        {
            var ab = _character.GetComponents<Ability>();

            foreach (var ability in ab)
            {
                abilities.Add(ability);
                ability.manager = this;
            }

            abilities = abilities.OrderBy(x => x.id).ToList();
        }
        private void UpdateHealthUI()
        {
            _healthBar.UpdateBar(CharacterStats.Health, CharacterStats.MaxHealth);
            onHealthChange?.Invoke(CharacterStats.Health, CharacterStats.MaxHealth);
        }
        
        public virtual void NotifyUIStats()
        {
            
        }
        
        public virtual bool Damage(float damage)
        {
            if (_isAlive == false) return true;
            if (Networking.NetworkData.NetSocket.NetCon == Networking.NetCon.Client) return false;

            var finalDamage =  Mathf.Max(1f, damage - CharacterStats.Shield);
            CharacterStats.Health -= finalDamage;
            if (CharacterStats.Health <= 0)
            {
                CharacterStats.Health = 0;
                Die();
                UpdateHealthUI();
                return true;
            }
            
            UpdateHealthUI();
            Debug.Log($"Damaging player manager with {finalDamage} damage, {CharacterStats.Health} health remaining");
            return false;
        }

        public void SetHealth(float health)
        {
            CharacterStats.Health = health;
            UpdateHealthUI();
        }

        public float GetHealth()
        {
            return CharacterStats.Health;
        }

        public virtual void Attack(IGameEntity enemy)
        {
            enemy.Damage(CharacterStats.TotalDamage);
            Debug.Log($"Player Attacking {CharacterStats.TotalDamage}");
            onAutoAttack?.Invoke();
        }
        
        public virtual void Die()
        {
            _isAlive = false;
            Destroy(_character);
            _character = null;
            _respawnCoroutine ??= StartCoroutine(RespawnCoroutine());

            // Send player death packet
            if (Networking.NetworkData.NetSocket.NetCon == Networking.NetCon.Host)
            {
                Networking.NPPlayerDeath packet = new();
                packet.net_id = _playerNetObj.NetID;

                Networking.ReplicationManager.Instance.SendPacket(packet);
            }
        }
        
        private IEnumerator RespawnCoroutine()
        {
            // TODO: Set the screen grayscale
            
            // Disable necessary components
            _healthBar.gameObject.SetActive(false);
            _healthBar.StopAllCoroutines();
            
            float timeToSpawn = BaseRespawnTime + (CharacterStats.Level - 1) * LevelRespawnMultiplier;
            Debug.Log($"Player respawn in {timeToSpawn} seconds");
            onPlayerDied?.Invoke(timeToSpawn);
            
            yield return new WaitForSeconds(timeToSpawn);
            
            // Disable necessary components
            _healthBar.gameObject.SetActive(true);
            
            // TODO: Set the screen to normal
            
            _character = characterData.Respawn(spawnPoint);
            CharacterStats.Health = CharacterStats.MaxHealth;
            NotifyUIStats();
            onPlayerSpawn?.Invoke();
            _isAlive = true;
            _respawnCoroutine = null;
        }

        public void Stun(float stunTime)
        {
            throw new NotImplementedException();
        }
    }
}
