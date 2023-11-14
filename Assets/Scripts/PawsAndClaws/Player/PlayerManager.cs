using PawsAndClaws.UI;
using System;
using System.Collections;
using PawsAndClaws.Entities;
using PawsAndClaws.Game;
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
        public CharacterStats CharacterStats { get => _characterStats; }
        private CharacterStats _characterStats;
        private GameObject _character;
        private bool _isAlive = true;
        
        // Components
        private PlayerCameraController _playerCameraController;
        private PlayerStateMachine _playerStateMachine;
        private InGameHealthBarUI _healthBar;
        private Camera _playerCameraComp;

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
    
        // Respawn variables
        private const float BaseRespawnTime = 30f;
        private const float LevelRespawnMultiplier = 2f;
        private Coroutine _respawnCoroutine;
        

        public string GetCurrentStateName() => _playerStateMachine.GetCurrentStateName();

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
        GameObject IGameEntity.GameObject { get => gameObject; set { } }

        private void Awake()
        {
            _playerCameraController = playerCamera.GetComponent<PlayerCameraController>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _playerCameraComp = playerCamera.GetComponent<Camera>();
            _healthBar = GetComponentInChildren<InGameHealthBarUI>();
            
            // Setup the reference for the player movement script
            _playerStateMachine.inputHandler = inputHandler;
            _playerStateMachine.playerCamera = _playerCameraComp;
            
            // Setup the reference for the camera controller script
            _playerCameraController.player = transform;
            _playerCameraController.inputHandler = inputHandler;


            // Spawn the character
            _character = characterData.Spawn(transform, ref _characterStats);

            GameManager.Instance.playerTeam = characterData.team;

            gameObject.layer = characterData.team == Team.Cat ?
                GameConstants.CatLayerMask:
                GameConstants.HamsterLayerMask;
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
            onStatsChanged?.Invoke(_characterStats);
        }

        
    
        public bool Damage(float damage)
        {
            var finalDamage =  Mathf.Max(1f, damage - _characterStats.Shield);
            _characterStats.Health -= finalDamage;
            if (_characterStats.Health <= 0)
            {
                _characterStats.Health = 0;
                Die();
                UpdateHealthUI();
                return true;
            }
            
            UpdateHealthUI();
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
            _isAlive = false;
            Destroy(_character);
            _character = null;
            _respawnCoroutine ??= StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            // TODO: Set the screen grayscale
            
            // Disable necessary components
            _playerStateMachine.enabled = false;
            _healthBar.gameObject.SetActive(false);
            _healthBar.StopAllCoroutines();
            
            float timeToSpawn = BaseRespawnTime + (_characterStats.Level - 1) * LevelRespawnMultiplier;
            Debug.Log($"Player respawn in {timeToSpawn} seconds");
            onPlayerDied?.Invoke(timeToSpawn);
            
            yield return new WaitForSeconds(timeToSpawn);
            
            // Disable necessary components
            _playerStateMachine.enabled = true;
            _healthBar.gameObject.SetActive(true);
            
            // TODO: Set the screen to normal
            
            _character = characterData.Respawn(transform);
            _characterStats.Health = _characterStats.MaxHealth;
            NotifyUIStats();
            onPlayerSpawn?.Invoke();
            _isAlive = true;
        }
        
        
        private void OnMouseOver()
        {
            if(GameManager.Instance.playerTeam != characterData.team)
                PlayerInputHandler.SetCursorAttack();
        }

        private void OnMouseExit()
        {
            if(GameManager.Instance.playerTeam != characterData.team)
                PlayerInputHandler.SetCursorDefault();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _characterStats.Range);
        }
    }
}
