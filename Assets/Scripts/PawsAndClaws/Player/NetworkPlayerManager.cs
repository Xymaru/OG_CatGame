using System;
using PawsAndClaws.Entities;
using PawsAndClaws.Game;
using PawsAndClaws.UI;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Player
{
    public class NetworkPlayerManager : PlayerManager
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private bool _wasRight = false;

        private void Start()
        {
            _healthBar = GetComponentInChildren<InGamePlayerHealthBarUI>();
            animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            // Spawn the character
            _character = characterData.Spawn(transform, ref CharacterStats);
            CollectAbilities();
            rigidBody = GetComponent<Rigidbody2D>();
            gameObject.layer = characterData.team == Team.Cat ?
                GameConstants.CatLayerMask:
                GameConstants.HamsterLayerMask;
            
            _healthBar.UpdateBar(CharacterStats.Health, CharacterStats.MaxHealth);
            _healthBar.UpdateName(userName);
        }

        private void Update()
        {
            if (!animator)
            {
                animator = GetComponentInChildren<Animator>();
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            float speed = Mathf.Abs(rigidBody.velocity.magnitude);

            Debug.Log(speed);

            if (animator != null)
                animator.SetFloat(GameConstants.SpeedAnim, speed);

            switch (rigidBody.velocity.x)
            {
                case > 0.1f when _wasRight:
                case < -0.1f when !_wasRight:
                    FlipX();
                    break;
            }
        }

        private void FlipX()
        {
            _wasRight = !_wasRight;
            _spriteRenderer.flipX = _wasRight;
        }

        public void SetPosition(Vector2 pos)
        {
            rigidBody.MovePosition(pos);
        }

        public void ActivateAbility(int ability, Networking.NetworkPacket packet)
        {
            if (abilities.Count <= ability)
                return;
            var ab = abilities[ability];
            ab.Activate(packet);
        }

        public void ActivateAbility1(Networking.NetworkPacket packet)
        {
            if (abilities.Count < 2)
                return;
            var ab = abilities[1];
            ab.Activate(packet);
        }
        public void ActivateAbility2(Networking.NetworkPacket packet)
        {
            if (abilities.Count < 3)
                return;
            var ab = abilities[2];
            ab.Activate(packet);
        }
        public void ActivateUltimate(Networking.NetworkPacket packet)
        {
            if (abilities.Count < 4)
                return;
            var ab = abilities[3];
            ab.Activate(packet);
        }

        public override void Attack(IGameEntity enemy)
        {
        }

        #region Gizmos
        private void OnMouseOver()
        {
            if(GameManager.Instance.PlayerTeam != characterData.team)
                PlayerInputHandler.SetCursorAttack();
        }

        private void OnMouseExit()
        {
            if(GameManager.Instance.PlayerTeam != characterData.team)
                PlayerInputHandler.SetCursorDefault();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, CharacterStats.Range);
        }
        #endregion
    }
}
