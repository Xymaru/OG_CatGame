using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities
{
    public class ActiveAbility : Ability
    {
        [SerializeField] protected float cooldown;
        [SerializeField] protected float cost;
        [SerializeField] protected float damage;
        
        private void Start()
        {
            cooldown = abilityInfo.Cooldown;
            cost = abilityInfo.Cost;
            damage = abilityInfo.Damage;
        }
    }
}
