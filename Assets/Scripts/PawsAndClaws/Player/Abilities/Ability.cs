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
        public Action onActivate;
        public virtual void Activate()
        {
            onActivate?.Invoke();
        }
    }
}
