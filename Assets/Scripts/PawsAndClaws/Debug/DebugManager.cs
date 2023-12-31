using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance { get; private set; }
        
        private void Start()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Instance != null)
            {
                Destroy(this);
            }

            Instance = this;
            DontDestroyOnLoad(this);
#else
            Destroy(this);
#endif
        }
    }
}
