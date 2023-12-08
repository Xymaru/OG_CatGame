using PawsAndClaws.Game;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PawsAndClaws.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;
        public static Texture2D DefaultCursorTexture;
        [SerializeField] private Texture2D attackCursorTexture;
        public static Texture2D AttackCursorTexture;

        [SerializeField] private Rigidbody2D rb;
        private Vector2 moveDirection;

        public int oppositeTeam;
        public Camera playerCamera;
        public PlayerInputManager InputManager { get; private set; }
        public LocalPlayerManager playerManager;
        public Animator animator;
        private SpriteRenderer _spriteRenderer;
        private bool _wasRight = false;

        public void Initialize()
        {
            InputManager = new PlayerInputManager();
            InputManager.Gameplay.Enable();
            Cursor.lockState = CursorLockMode.Confined;

            AttackCursorTexture = attackCursorTexture;
            DefaultCursorTexture = defaultCursorTexture;
            
            Cursor.SetCursor(defaultCursorTexture,  new Vector2(0.5f, 0.5f), CursorMode.Auto);


            rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = animator.GetComponent<SpriteRenderer>();
            
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
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            if(animator != null)
                animator.SetFloat(GameConstants.SpeedAnim, Mathf.Abs(rb.velocity.magnitude));

            switch (rb.velocity.x)
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

        private void UpdateInput()
        {
            var moveX = Input.GetAxisRaw("Horizontal");
            var moveY = Input.GetAxisRaw("Vertical");

            moveDirection = new Vector2 (moveX, moveY).normalized;
        }

        public void FixedUpdate()
        {
            if (playerManager == null)
                return;

            Move();
        }

        void Move()
        {
            rb.velocity = new Vector2(moveDirection.x * playerManager.CharacterStats.Speed, moveDirection.y * playerManager.CharacterStats.Speed);
        }
    }
}
