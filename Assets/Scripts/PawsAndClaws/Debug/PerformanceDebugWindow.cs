using ImGuiNET;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class PerformanceDebugWindow : DebugWindow
    {

        private List<float> _fpsCounter = new List<float>();
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
            ImGui.Begin("Perfomance");

            ImGui.PlotHistogram("FPS", ref _fpsCounter.ToArray()[0], _fpsCounter.Count);
            ImGui.Text($"FPS: {Time.deltaTime * 1000}ms");

            ImGui.End();
        }
    }
}
