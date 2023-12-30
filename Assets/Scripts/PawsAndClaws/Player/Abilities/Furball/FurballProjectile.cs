using PawsAndClaws.Entities;
using System;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities.Furball
{
    public class FurballProjectile : Projectile
    {
        protected override void OnCollisionWithOtherEnt(IGameEntity other)
        {
            if (other != null)
            {
                other.Damage(damage);
            }
        }
    }
}
