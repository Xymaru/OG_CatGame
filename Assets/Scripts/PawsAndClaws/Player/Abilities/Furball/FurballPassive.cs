using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities.Furball
{
    public class FurballPassive : Ability
    {
        [SerializeField] private int attacksToTrigger;
        private int _attackCount;

        

        public override void Activate()
        {
            _attackCount++;
            var mult = manager.CharacterStats.DamageMultiplier;
            if(_attackCount > attacksToTrigger)
            {
                base.Activate();
                _attackCount = 0;
                manager.CharacterStats.DamageMultiplier = 2;
                return;
            }
            manager.CharacterStats.DamageMultiplier = mult;
        }
    }
}
