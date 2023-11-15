using ImGuiNET;
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
        private float _fpsLimit = 144f;
        private void Update()
        {
            _fpsCounter.Add(Time.deltaTime * 1000);
            
            if(_fpsCounter.Count > 120 )
            {
                _fpsCounter.RemoveAt(0);
            }
        }

        public override void OnImGuiRenderer()
        {
            ImGui.Begin("Performance");
            
            if (_fpsCounter.Count > 0)
            {
                ImGui.PlotHistogram("FPS", ref _fpsCounter.ToArray()[0], _fpsCounter.Count);
            }
            ImGui.Text($"FPS: {Time.deltaTime * 1000}ms");

            if (ImGui.Checkbox("Limit FPS", ref _limitFPS))
            {
                if (!_limitFPS)
                {
                    Application.targetFrameRate = 0;
                }
            }
            if (ImGui.DragFloat("FPS limit", ref _fpsLimit))
            {
                _limitFPS = true;
                Application.targetFrameRate = (int)_fpsLimit;
            }
            
            ImGui.End();
        }
    }
}
