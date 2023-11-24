using System.Collections.Generic;
using System.Linq;
using PawsAndClaws.Game;
using PawsAndClaws.Player.Abilities;
using PawsAndClaws.UI;
using UnityEngine;
using UnityEngine.AI;

namespace PawsAndClaws.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class LocalPlayerManager : PlayerManager
    {
        // Player object references
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private PlayerInputHandler inputHandler;
        
        // Components
        private PlayerCameraController _playerCameraController;
        private Camera _playerCameraComp;
        
        private void Start()
        {
            _playerCameraController = playerCamera.GetComponent<PlayerCameraController>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _playerCameraComp = playerCamera.GetComponent<Camera>();
            _healthBar = GetComponentInChildren<InGamePlayerHealthBarUI>();
            rigidBody = GetComponent<Rigidbody2D>();
            // Setup the reference for the player movement script
            inputHandler.playerCamera = _playerCameraComp;
            inputHandler.playerStateMachine = _playerStateMachine;
            inputHandler.playerManager = this;
            
            // Setup the reference for the camera controller script
            _playerCameraController.player = transform;
            _playerCameraController.inputHandler = inputHandler;


            // Spawn the character
            _character = characterData.Spawn(transform, ref CharacterStats);
            CollectAbilities();
            _playerStateMachine.Start();
            
            inputHandler.Initialize();
            _playerCameraController.Initialize();
            
            GameManager.Instance.playerTeam = characterData.team;

             gameObject.layer = characterData.team == Team.Cat ?
                GameConstants.CatLayerMask:
                GameConstants.HamsterLayerMask;
             inputHandler.oppositeTeam = characterData.team == Team.Cat ?
                 GameConstants.HamsterLayerMask:
                 GameConstants.CatLayerMask;
            
            // Update the UI
            NotifyUIStats();
        }


        public void ActivateAbility1()
        {
            if (abilities.Count < 2)
                return;
            var ab = abilities[1];
            ab.Activate();
        }
        public void ActivateAbility2()
        {
            
        }
        public void ActivateUltimate()
        {
            
        }
        
        protected override void NotifyUIStats()
        {
            _healthBar.UpdateBar(CharacterStats.Health, CharacterStats.MaxHealth);
            _healthBar.UpdateName(userName);
            onHealthChange?.Invoke(CharacterStats.Health, CharacterStats.MaxHealth);
            onHealthRegenChange?.Invoke(CharacterStats.HealthRegen);
            onManaChange?.Invoke(CharacterStats.Mana, CharacterStats.MaxMana);
            onManaRegenChange?.Invoke(CharacterStats.ManaRegen);
            onExpChange?.Invoke(CharacterStats.Experience, CharacterStats.ExpToNextLevel);
            onLevelUp?.Invoke(CharacterStats.Level);
            onStatsChanged?.Invoke(CharacterStats);
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
