using UnityEngine;

namespace PawsAndClaws.Game
{
    public static class GameConstants 
    {
        #region Player

        public static string UserName = "Unknown";
        public static bool UserNameSet = false;

        #endregion
        
        #region Layers
        public static readonly LayerMask HamsterLayerMask = LayerMask.NameToLayer("Hamsters");
        public static readonly LayerMask CatLayerMask = LayerMask.NameToLayer("Cats");
        #endregion
        
        #region Animations

        public static readonly int AutoAttackAnim = Animator.StringToHash("AutoAttack");
        public static readonly int SpeedAnim = Animator.StringToHash("Speed");
        
        #endregion
    }
}
