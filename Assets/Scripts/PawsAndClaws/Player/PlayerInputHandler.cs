using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D attackCursorTexture;
        
        public Camera playerCamera;
        public PlayerStateMachine playerStateMachine;
        public static Texture2D DefaultCursorTexture;
        public static Texture2D AttackCursorTexture;
        public PlayerInputManager InputManager { get; private set; }
        
        private LayerMask _oppositeTeam;
        private void OnEnable()
        {
            InputManager.Gameplay.Enable();
        }

        private void Awake()
        {
            InputManager = new PlayerInputManager();
            Cursor.lockState = CursorLockMode.Confined;

            AttackCursorTexture = attackCursorTexture;
            DefaultCursorTexture = defaultCursorTexture;
            
            Cursor.SetCursor(defaultCursorTexture,  new Vector2(0.5f, 0.5f), CursorMode.Auto);
            
            InputManager.Gameplay.Move.performed += c => HandlePlayerMoveInput();
            
            _oppositeTeam = Utils.GameUtils.GetOppositeLayer(gameObject);
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
            var hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, _oppositeTeam);
            if (hit.collider != null)
            {
                Debug.Log("Player requested attack");
                playerStateMachine.movingState.doAttack = true;
                var target = Utils.GameUtils.GetIfHasIGameEntity(hit.collider.gameObject);
                if(target is { IsAlive: true })
                {
                    playerStateMachine.enemyTarget = target;
                }
            }
            else
            {
                playerStateMachine.movingState.doAttack = false;
            }
            
            playerStateMachine.target = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            playerStateMachine.movingState.target = playerStateMachine.target;

            playerStateMachine.ChangeState(playerStateMachine.movingState);
        }
    }
}
