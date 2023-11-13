using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws.Entities
{
    public interface IGameEntity
    {
        public Team Team { get; set; }
        public bool IsAlive { get; protected set; }

        public GameObject GameObject { get; protected set; }
        
        // Returns true if the entity died
        public abstract bool Damage(float damage);
        public abstract void Die();
    }
}