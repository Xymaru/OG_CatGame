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
        private void Start()
        {
            _healthBar = GetComponentInChildren<InGamePlayerHealthBarUI>();

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
