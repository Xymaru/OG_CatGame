using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities.Furball
{
    public class FurballAbFurrball : ActiveAbility
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletForce = 20f;
        private float time = 0f;
        

        public override void Activate(int cooldown = 0)
        {
            if (manager.CharacterStats.Mana < cost || time > cooldown)
            {
                return;
            }
            
            base.Activate(cooldown);
            manager.CharacterStats.Mana -= cost;
            time = cooldown;
            
            // Calculate the direction from the player to the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - transform.position).normalized;

            // Instantiate a new bullet at the fire point
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            // Get the Rigidbody2D component of the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // Apply force to the bullet in the direction of the mouse
            rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
            
            // Set the damage to the projectile
            FurballProjectile projectile = bullet.GetComponent<FurballProjectile>();
            projectile.team = manager.Team;
            projectile.damage = manager.CharacterStats.TotalDamage * damage;
            projectile.owner = manager;
        }

        private void Update()
        {
            time -= Time.deltaTime;
        }
    }
}
