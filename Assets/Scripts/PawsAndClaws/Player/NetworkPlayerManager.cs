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
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _agent = GetComponent<NavMeshAgent>();
            
            // Spawn the character
            _character = characterData.Spawn(transform, ref CharacterStats);
            CollectAbilities();
            _playerStateMachine.Start();
            
            gameObject.layer = characterData.team == Team.Cat ?
                GameConstants.CatLayerMask:
                GameConstants.HamsterLayerMask;
            
            _healthBar.UpdateBar(CharacterStats.Health, CharacterStats.MaxHealth);
            _healthBar.UpdateName(userName);
        }

        public void SetPosition(Vector2 pos)
        {
            _agent.nextPosition = new Vector3(pos.x, pos.y, 0f);
        }
        
        public override bool Damage(float damage)
        {
            var finalDamage =  Mathf.Max(1f, damage - CharacterStats.Shield);
            CharacterStats.Health -= finalDamage;
            if (CharacterStats.Health <= 0)
            {
                CharacterStats.Health = 0;
                Die();
                _healthBar.UpdateBar(CharacterStats.Health, CharacterStats.MaxHealth);
                return true;
            }
            
            _healthBar.UpdateBar(CharacterStats.Health, CharacterStats.MaxHealth);
            Debug.Log($"Damaging remote player manager with {finalDamage} damage, {CharacterStats.Health} health remaining");
            return false;  
        }

        public override void Attack(IGameEntity enemy)
        {
        }

        public override void Die()
        {
            
        }
        
        #region Gizmos
        private void OnMouseOver()
        {
            if(GameManager.Instance.playerTeam != characterData.team)
                PlayerInputHandler.SetCursorAttack();
        }

        private void OnMouseExit()
        {
            if(GameManager.Instance.playerTeam != characterData.team)
                PlayerInputHandler.SetCursorDefault();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, CharacterStats.Range);
        }
        #endregion
    }
}
