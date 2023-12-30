using PawsAndClaws.Networking;
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
        
        private void _activate_ability(float cooldown, Vector2 direction)
        {
            time = this.cooldown;
            manager.CharacterStats.Mana -= cost;
            manager.onManaChange?.Invoke(manager.CharacterStats.Mana, manager.CharacterStats.MaxMana);
            base.Activate(this.cooldown);

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

        public override void Activate(float cooldown = 0)
        {
            if (manager.CharacterStats.Mana < cost || time > 0f)
            {
                return;
            }

            // Calculate the direction from the player to the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - transform.position).normalized;

            _activate_ability(cooldown, direction);

            // Replicate ability spawn
            GameObject player = NetworkData.NetSocket.PlayerI.player_obj;
            int net_id = player.GetComponentInParent<NetworkObject>().NetID;

            NPAbility ab_packet = new();
            ab_packet.net_id = net_id;
            ab_packet.ab_number = 1;
            ab_packet.ab_direction = direction;

            ReplicationManager.Instance.SendPacket(ab_packet);
        }

        public override void Activate(NetworkPacket packet, float cooldown = 0)
        {
            NPAbility ab_packet = packet as NPAbility;

            _activate_ability(cooldown, ab_packet.ab_direction);
        }

        private void Update()
        {
            time -= Time.deltaTime;

        }
    }
}
