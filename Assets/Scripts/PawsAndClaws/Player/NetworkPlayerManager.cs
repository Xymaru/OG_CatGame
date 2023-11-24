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
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
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
            transform.position = new Vector3(pos.x, pos.y, 0f);
        }
        

        public override void Attack(IGameEntity enemy)
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
