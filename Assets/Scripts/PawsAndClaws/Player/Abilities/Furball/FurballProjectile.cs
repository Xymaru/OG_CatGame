using System;
using UnityEngine;

namespace PawsAndClaws.Player.Abilities.Furball
{
    public class FurballProjectile : MonoBehaviour
    {
        public Team team;
        public float damage;
        public GameObject owner;
        public float destroyTime = 2f;
        private float time = 0f;
        public void Update()
        {
            time += Time.deltaTime;
            if (time > destroyTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = Utils.GameUtils.CheckIfOtherTeam(other.gameObject, team);
            if (target != null)
            {
                Debug.Log("Damage");
                target.Damage(damage);
            }
        }
    }
}
