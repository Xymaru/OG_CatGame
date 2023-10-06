using PawsAndClaws.UI;
using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public struct CharacterStats
    {
        private const int MaxLevel = 9;

        public float Health;
        public float MaxHealth;
        public float HealthRegen;

        public float Mana;
        public float MaxMana;
        public float ManaRegen;

        public float Damage;
        public float DamageMultiplier;

        public float Shield;
        public float ShieldMultiplier;

        public int Level;
        public int Experience;
        public int ExpToNextLevel;

        // Multipliers for passives / ultimates
        public float HealthRegenMultiplier;
        public float ManaRegenMultiplier;

        private CharacterDataSO _data;

        public void Initialize(CharacterDataSO data)
        {
            _data = data;
            Health = MaxHealth = data.startingHealth;
            HealthRegen = data.healthRegen;
            Mana = MaxMana = data.startingMana;
            ManaRegen = data.manaRegen;
            Damage = data.startingDamage;
            Shield = data.startingShield;

            Level = 1;
            Experience = 0;
            CalcNextLevelExp();
            HealthRegenMultiplier = 1;
            ManaRegenMultiplier = 1;
        }


        public void AddXp(int xp)
        {
            Experience += xp;

            if (Experience >= ExpToNextLevel && Level >= MaxLevel)
                LevelUp();
        }

        private void LevelUp()
        {
            Level++;
            if (Level >= MaxLevel)
                Level = MaxLevel;

            Experience = 0;

            CalcNextLevelExp();
            UpgradeStats();
        }

        private void UpgradeStats()
        {
            Health *= _data.healthLevelMultiplier;
            MaxHealth *= _data.healthLevelMultiplier;
            HealthRegen *= _data.healthRegenLevelMultiplier;

            Mana *= _data.manaLevelMultiplier;
            MaxMana *= _data.manaLevelMultiplier;
            ManaRegen *= _data.manaRegenLevelMultiplier;

            Damage *= _data.damageLevelMultiplier;
            Shield *= _data.shieldLevelMultiplier;
        }

        private void CalcNextLevelExp()
        {
            ExpToNextLevel = (int)((ExpToNextLevel + 10) * 1.1f);
        }
    }


    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private PlayerInputHandler inputHandler;
        public CharacterDataSO characterData;
        private CharacterStats _characterStats;

        private PlayerCameraController _playerCameraController;
        private PlayerMovementController _playerMovementController;
        private Camera _playerCameraComp;
        private GameObject _character;

        public Action<float, float> onHealthChange;
        public Action<float> onHealthRegenChange;
        public Action<float, float> onManaChange;
        public Action<float> onManaRegenChange;
        
        public Action<float, float> onExpChange;
        public Action<int> onLevelUp;

        private void Awake()
        {
            _playerCameraController = playerCamera.GetComponent<PlayerCameraController>();
            _playerMovementController = player.GetComponent<PlayerMovementController>();
            _playerCameraComp = playerCamera.GetComponent<Camera>();
            
            // Setup the reference for the player movement script
            _playerMovementController.inputHandler = inputHandler;
            _playerMovementController.playerCamera = _playerCameraComp;
            
            // Setup the reference for the camera controller script
            _playerCameraController.player = player.transform;
            _playerCameraController.inputHandler = inputHandler;


            // Spawn the character
            _character = characterData.Spawn(player.transform, ref _characterStats);

            // Update the UI
            NotifyUIStats();
        }

        private void NotifyUIStats()
        {
            onHealthChange?.Invoke(_characterStats.Health, _characterStats.MaxHealth);
            onHealthRegenChange?.Invoke(_characterStats.HealthRegen);
            onManaChange?.Invoke(_characterStats.Mana, _characterStats.MaxMana);
            onManaRegenChange?.Invoke(_characterStats.ManaRegen);
            onExpChange?.Invoke(_characterStats.Experience, _characterStats.ExpToNextLevel);
            onLevelUp?.Invoke(_characterStats.Level);
        }

    }
}
