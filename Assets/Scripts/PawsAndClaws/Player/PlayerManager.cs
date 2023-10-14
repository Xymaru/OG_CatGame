using PawsAndClaws.UI;
using System;
using System.Collections;
using PawsAndClaws.Entities;
using UnityEngine;

namespace PawsAndClaws.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerManager : MonoBehaviour, IGameEntity
    {
        // Player object references
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private PlayerInputHandler inputHandler;
        
        // Character data
        public CharacterDataSO characterData;
        private CharacterStats _characterStats;
        private GameObject _character;
        private bool _isAlive = true;
        
        // Components
        private PlayerCameraController _playerCameraController;
        private PlayerMovementController _playerMovementController;
        private InGameHealthBarUI _healthBar;
        private Camera _playerCameraComp;

        // Events
        public Action<float, float> onHealthChange;
        public Action<float> onHealthRegenChange;
        public Action<float, float> onManaChange;
        public Action<float> onManaRegenChange;
        public Action<float, float> onExpChange;
        public Action<int> onLevelUp;
    
        // Respawn variables
        private const float BaseRespawnTime = 30f;
        private const float LevelRespawnMultiplier = 2f;
        private Coroutine _respawnCoroutine;
       

        Team IGameEntity.Team
        {
            get => characterData.team;
            set { }
        }

        bool IGameEntity.IsAlive
        {
            get => _isAlive;
            set => _isAlive = value;
        }

        private void Awake()
        {
            _playerCameraController = playerCamera.GetComponent<PlayerCameraController>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            _playerCameraComp = playerCamera.GetComponent<Camera>();
            _healthBar = GetComponentInChildren<InGameHealthBarUI>();
            
            // Setup the reference for the player movement script
            _playerMovementController.inputHandler = inputHandler;
            _playerMovementController.playerCamera = _playerCameraComp;
            
            // Setup the reference for the camera controller script
            _playerCameraController.player = transform;
            _playerCameraController.inputHandler = inputHandler;


            // Spawn the character
            _character = characterData.Spawn(transform, ref _characterStats);
        }

        private void Start()
        {
            // Update the UI
            NotifyUIStats();
        }

        private void NotifyUIStats()
        {
            _healthBar.UpdateBar(_characterStats.Health, _characterStats.MaxHealth);   
            onHealthChange?.Invoke(_characterStats.Health, _characterStats.MaxHealth);
            onHealthRegenChange?.Invoke(_characterStats.HealthRegen);
            onManaChange?.Invoke(_characterStats.Mana, _characterStats.MaxMana);
            onManaRegenChange?.Invoke(_characterStats.ManaRegen);
            onExpChange?.Invoke(_characterStats.Experience, _characterStats.ExpToNextLevel);
            onLevelUp?.Invoke(_characterStats.Level);
        }

        
    
        public bool Damage(float damage)
        {
            var finalDamage =  Mathf.Max(1f, damage - _characterStats.Shield);
            _characterStats.Health -= finalDamage;
            UpdateHealthUI();
            
            if (_characterStats.Health <= 0)
            {
                _characterStats.Health = 0;
                Die();
                return true;
            }
            Debug.Log($"Damaging player manager with {finalDamage} damage, {_characterStats.Health} health remaining");
            return false;
        }

        private void UpdateHealthUI()
        {
            _healthBar.UpdateBar(_characterStats.Health, _characterStats.MaxHealth);
            onHealthChange?.Invoke(_characterStats.Health, _characterStats.MaxHealth);
        }

        public void Die()
        {
            Destroy(_character);
            _character = null;
            _respawnCoroutine ??= StartCoroutine(RespawnCoroutine());
            _isAlive = false;
        }

        private IEnumerator RespawnCoroutine()
        {
            // TODO: Set the screen grayscale
            
            // Disable necessary components
            _playerMovementController.enabled = false;
            _healthBar.gameObject.SetActive(false);
            _healthBar.StopAllCoroutines();
            
            float timeToSpawn = BaseRespawnTime + (_characterStats.Level - 1) * LevelRespawnMultiplier;
            Debug.Log($"Player respawn in {timeToSpawn} seconds");
            // TODO: Notify UI
            yield return new WaitForSeconds(timeToSpawn);
            
            // Disable necessary components
            _playerMovementController.enabled = true;
            _healthBar.gameObject.SetActive(true);
            
            // TODO: Set the screen to normal
            _character = characterData.Respawn(transform);
            _characterStats.Health = _characterStats.MaxHealth;
            
            _playerMovementController.enabled = false;
            _isAlive = true;
        }
    }
}
