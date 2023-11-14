using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws
{
    public static class GameConstants 
    {
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
