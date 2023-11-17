using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities.Furball
{
    public class FurballPassive : Ability
    {
        [SerializeField] private int attacksToTrigger;
        private int _attackCount;

        private void OnEnable()
        {
            manager.onAutoAttack += Activate;
        }

        private void OnDisable()
        {
            manager.onAutoAttack -= Activate;
        }

        public override void Activate()
        {
            _attackCount++;

            if(_attackCount > attacksToTrigger)
            {
                base.Activate();
                _attackCount = 0;
                manager.CharacterStats.DamageMultiplier = 2;

                return;
            }

        }
    }
}
