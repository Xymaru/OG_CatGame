using System;
using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws.Entities
{
    public interface IGameEntity
    {
        public Team Team { get; set; }
        public bool IsAlive { get; set; }

        public GameObject GameObject { get; protected set; }
        
        // Returns true if the entity died
        public abstract bool Damage(float damage);
        public abstract void Die();

        public abstract void Stun(float stunTime);
        
    }
}