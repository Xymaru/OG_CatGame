using PawsAndClaws.Player;

namespace PawsAndClaws.Entities
{
    public interface IGameEntity
    {
        public Team Team { get; protected set; }
        public bool IsAlive { get; protected set; }
        
        // Returns true if the entity died
        public abstract bool Damage(float damage);
        public abstract void Die();
    }
}