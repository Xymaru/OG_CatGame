using System;
using UnityEngine;

namespace PawsAndClaws.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerInputManager InputManager { get; private set; }

        private void OnEnable()
        {
            InputManager.Gameplay.Enable();
        }

        private void Awake()
        {
            InputManager = new PlayerInputManager();
        }

        private void OnDisable()
        {
            InputManager.Gameplay.Disable();
        }
    }
}
