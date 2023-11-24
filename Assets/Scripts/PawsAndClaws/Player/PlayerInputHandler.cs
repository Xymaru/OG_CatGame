using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PawsAndClaws.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D attackCursorTexture;

        [SerializeField] private Rigidbody2D rb;

        public int oppositeTeam;
        public Camera playerCamera;
        public PlayerStateMachine playerStateMachine;
        public static Texture2D DefaultCursorTexture;
        public static Texture2D AttackCursorTexture;
        
        public PlayerInputManager InputManager { get; private set; }

        public LocalPlayerManager playerManager;

        Vector2 movement;
        public void Initialize()
        {
            InputManager = new PlayerInputManager();
            InputManager.Gameplay.Enable();
            Cursor.lockState = CursorLockMode.Confined;

            AttackCursorTexture = attackCursorTexture;
            DefaultCursorTexture = defaultCursorTexture;
            
            Cursor.SetCursor(defaultCursorTexture,  new Vector2(0.5f, 0.5f), CursorMode.Auto);


            rb = GetComponent<Rigidbody2D>();

           
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


        public void Update()
        {
            UpdateInput();
        }

        private void UpdateInput()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        public void FixedUpdate()
        {
            if (playerManager == null)
                return;

            HandlePlayerMoveInput();
        }

        void HandlePlayerMoveInput()
        {
            rb.MovePosition(rb.position + movement * playerManager.CharacterStats.Speed * Time.fixedDeltaTime);
        }
    }
}
