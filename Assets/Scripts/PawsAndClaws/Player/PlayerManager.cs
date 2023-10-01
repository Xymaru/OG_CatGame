using UnityEngine;

namespace PawsAndClaws.Player
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private PlayerInputHandler inputHandler;

        private PlayerCameraController _playerCameraController;
        private PlayerMovementController _playerMovementController;
        private Camera _playerCameraComp;
        
        void Awake()
        {
            _playerCameraController = playerCamera.GetComponent<PlayerCameraController>();
            _playerMovementController = player.GetComponent<PlayerMovementController>();
            _playerCameraComp = playerCamera.GetComponent<Camera>();
            
            // Setup the reference for the player movement script
            _playerMovementController.inputHandler = inputHandler;
            _playerMovementController.playerCamera = _playerCameraComp;
            
            // Setup the reference for the camera controller script
            _playerCameraController.player = player.transform;
            _playerCameraController.inputHandler = inputHandler;
        }
    }
}
