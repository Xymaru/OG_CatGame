using PawsAndClaws.UI;
using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public struct CharacterStats
    {
        public const int maxLevel = 9;

        public float health;
        public float maxHealth;
        public float healthRegen;

        public float mana;
        public float maxMana;
        public float manaRegen;

        public float damage;
        public float damageMultiplier;

        public float shield;
        public float shieldMultiplier;

        public int level;
        public int experience;
        public int expToNextLevel;

        // Multipliers for passives / ultimates
        public float healthRegenMultiplier;
        public float manaRegenMultiplier;

        private CharacterDataSO data;

        public void Initialize(CharacterDataSO data)
        {
            this.data = data;
            health = maxHealth = data.startingHealth;
            healthRegen = data.healthRegen;
            mana = maxMana = data.startingMana;
            manaRegen = data.manaRegen;
            damage = data.startingDamage;
            shield = data.startingShield;

            level = 1;
            experience = 0;
            CalcNextLevelExp();
            healthRegenMultiplier = 1;
            manaRegenMultiplier = 1;
        }


        public void AddXP(int xp)
        {
            experience += xp;

            if (experience >= expToNextLevel && level >= maxLevel)
                LevelUp();
        }

        void LevelUp()
        {
            level++;
            if (level >= maxLevel)
                level = maxLevel;

            experience = 0;

            CalcNextLevelExp();
            UgradeStats();
        }

        void UgradeStats()
        {
            health *= data.healthLevelMultiplier;
            maxHealth *= data.healthLevelMultiplier;
            healthRegen *= data.healthRegenLevelMultiplier;

            mana *= data.manaLevelMultiplier;
            maxMana *= data.manaLevelMultiplier;
            manaRegen *= data.manaRegenLevelMultiplier;

            damage *= data.damageLevelMultiplier;
            shield *= data.shieldLevelMultiplier;
        }

        void CalcNextLevelExp()
        {
            expToNextLevel = (int)((expToNextLevel + 10) * 1.1f);
        }
    }


    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private PlayerInputHandler inputHandler;

        public CharacterDataSO characterData;
        public CharacterStats characterStats;

        private PlayerCameraController _playerCameraController;
        private PlayerMovementController _playerMovementController;
        private Camera _playerCameraComp;

        public Action<float, float> onHealthChange;
        public Action<float, float> onManaChange;
        public Action<float, float> onExpChange;
        public Action<int> onLevelUp;

        void Awake()
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
            characterData.Spawn(player.transform, ref characterStats);

            // Update the UI
            NotifyUIStats();
        }

        public void NotifyUIStats()
        {
            onHealthChange?.Invoke(characterStats.health, characterStats.maxHealth);
            onManaChange?.Invoke(characterStats.mana, characterStats.maxMana);
            onExpChange?.Invoke(characterStats.experience, characterStats.expToNextLevel);
            onLevelUp?.Invoke(characterStats.level);
        }

    }
}
