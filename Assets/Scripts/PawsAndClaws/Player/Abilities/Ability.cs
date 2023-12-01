using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities
{
    public class Ability : MonoBehaviour
    {
        public bool active;
        public PlayerManager manager;
        public Action<float> onActivate;
        public AbilityInfoSO abilityInfo;
        public int id;


        public virtual void Activate(float cooldown = 0)
        {
            onActivate?.Invoke(cooldown);
        }
    }
}
