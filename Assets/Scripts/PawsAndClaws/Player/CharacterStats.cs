﻿namespace PawsAndClaws.Player
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
}