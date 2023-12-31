using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player
{
    [System.Serializable]
    public enum Team : byte
    {
        Cat,
        Hamster,
        None
    }

    [System.Serializable]
    public enum AttackType : byte
    {
        Melee,
        Ranged
    }

    [CreateAssetMenu(fileName = "Character Data", menuName = "Objects/CharacterData")]
    public class CharacterDataSO : ScriptableObject
    {
        public new string name;
        
        public Sprite sprite;
        public Sprite passiveImage;
        public Sprite ability1Sprite;
        public Sprite ability2Sprite;
        public Sprite ultimateSprite;

        public Team team;

        public float speed;

        public float startingHealth;
        public float healthLevelMultiplier;
        public float healthRegen;
        public float healthRegenLevelMultiplier;
        
        public AttackType attackType;

        public float startingDamage;
        public float damageLevelMultiplier;


        public float startingShield;
        public float shieldLevelMultiplier;

        public float startingMana;
        public float manaLevelMultiplier;
        public float manaRegen;
        public float manaRegenLevelMultiplier;

        public float range;

        public float attackSpeed;
        public float attackSpeedMultiplier;

        public GameObject prefab;

        public GameObject Spawn(Transform transform, ref CharacterStats stats)
        {
            var character = Instantiate(prefab, transform);
            stats.Initialize(this);

            return character;
        }

        public GameObject Respawn(Transform transform)
        {
            return Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}