
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class PerformanceDebugWindow : DebugWindow
    {
        private readonly List<float> _fpsCounter = new();
        private bool _limitFPS = false;
        private float _fpsLimit = 0f;
        private float _timeScale = 1f;
           
        private void Update() 
        {
            _fpsCounter.Add(Time.deltaTime * 1000);
            
            if(_fpsCounter.Count > 120)
            {
                _fpsCounter.RemoveAt(0);
            }
        }

        public override void OnImGuiRenderer()
        {

        }
    }
}
