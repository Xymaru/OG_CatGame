using UnityEngine;

namespace PawsAndClaws.Player.Abilities
{
    [CreateAssetMenu(fileName = "Ability info", menuName = "Objects/Ability info")]
    public class AbilityInfoSO : ScriptableObject
    {
        public string Name;
        public string Description;
        public float Cooldown;
        public float Cost;
        public float Damage;
    }
}
