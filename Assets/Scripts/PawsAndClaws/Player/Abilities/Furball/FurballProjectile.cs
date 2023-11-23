using System;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities.Furball
{
    public class FurballProjectile : MonoBehaviour
    {
        public Team team;
        public float damage;
        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = Utils.GameUtils.CheckIfOtherTeam(other.gameObject, team);
            if (target != null)
            {
                target.Damage(damage);
            }
            
            Destroy(gameObject);
        }
    }
}
