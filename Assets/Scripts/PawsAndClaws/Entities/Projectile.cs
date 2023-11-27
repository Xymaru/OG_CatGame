using PawsAndClaws.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Entities
{
    public class Projectile : MonoBehaviour
    {
        public IGameEntity owner;

        [SerializeField] private GameObject particle;
        public Team team;
        public float damage;
        public float destroyTime = 2f;
        private float _time = 0f;
        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update() 
        {
            UpdateForward();
            UpdateDestruction();   
        }

        private void UpdateForward()
        {
            var v = _rb.velocity;
            var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void UpdateDestruction()
        {
            _time += Time.deltaTime;
            if (_time > destroyTime)
            {
                Destroy(gameObject);
            }
        }

  
        private void OnCollisionEnter2D(Collision2D collision)
        {
            IGameEntity target = null;
            bool isOtherTeam = Utils.GameUtils.CheckIfOtherTeam(collision.gameObject, team, ref target);
            if (isOtherTeam)
            {
                OnCollisionWithOtherEnt(target);
            }
            if (target == owner)
                return;
            
            Destroy(gameObject);
        }

        protected virtual void OnCollisionWithOtherEnt(IGameEntity other)
        {

        }

        private void OnDestroy()
        {
            Instantiate(particle, transform.position, transform.rotation);
        }
    }
}