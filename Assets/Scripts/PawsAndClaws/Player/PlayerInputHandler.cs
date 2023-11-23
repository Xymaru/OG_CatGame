using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D attackCursorTexture;
        
        public int oppositeTeam;
        public Camera playerCamera;
        public PlayerStateMachine playerStateMachine;
        public static Texture2D DefaultCursorTexture;
        public static Texture2D AttackCursorTexture;
        public PlayerInputManager InputManager { get; private set; }

        public LocalPlayerManager playerManager;
        public void Initialize()
        {
            InputManager = new PlayerInputManager();
            InputManager.Gameplay.Enable();
            Cursor.lockState = CursorLockMode.Confined;

            AttackCursorTexture = attackCursorTexture;
            DefaultCursorTexture = defaultCursorTexture;
            
            Cursor.SetCursor(defaultCursorTexture,  new Vector2(0.5f, 0.5f), CursorMode.Auto);
            
            InputManager.Gameplay.Move.performed += c => HandlePlayerMoveInput();
            InputManager.Gameplay.Ability1.performed += c => playerManager.ActivateAbility1();
            InputManager.Gameplay.Ability2.performed += c => playerManager.ActivateAbility2();
            InputManager.Gameplay.Ultimate.performed += c => playerManager.ActivateUltimate();
        }

        public static void SetCursorAttack()
        {
            Cursor.SetCursor(AttackCursorTexture,  new Vector2(0.5f, 0.5f), CursorMode.Auto);
        }

        public static void SetCursorDefault()
        {
            Cursor.SetCursor(DefaultCursorTexture,  new Vector2(0.5f, 0.5f), CursorMode.Auto);
        }
        
        private void OnDisable()
        {
            InputManager.Gameplay.Disable();
        }
        
        void HandlePlayerMoveInput()
        {
            // Check if the player wants to attack
            var ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, 1000);
            if (hit.collider != null)
            {
                var target = Utils.GameUtils.GetIfIsEntityFromOtherTeam(gameObject, hit.collider.gameObject);
                if(target is { IsAlive: true })
                {
                    Debug.Log("Player requested attack");
                    playerStateMachine.EnemyTarget = target;
                    playerStateMachine.ChangeState(playerStateMachine.ChaseState);
                }
            }
            else
            {
                playerStateMachine.ChangeState(playerStateMachine.MovingState);
            }
            playerStateMachine.target = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            playerStateMachine.MovingState.target = playerStateMachine.target;
        }
    }
}
