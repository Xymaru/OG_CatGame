using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D attackCursorTexture;
        
        
        public static Texture2D DefaultCursorTexture;
        public static Texture2D AttackCursorTexture;
        public PlayerInputManager InputManager { get; private set; }
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
    }
}
