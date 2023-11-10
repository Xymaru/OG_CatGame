using ImGuiNET;
using ImGuiNET.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Debugging
{

    public class ImGuiManager : MonoBehaviour
    {
        [SerializeField] private List<DebugWindow> editorWindows = new List<DebugWindow>();

        private void OnEnable()
        {
            ImGuiUn.Layout += OnImGuiRender;
        }

        private void OnDisable()
        {
            ImGuiUn.Layout -= OnImGuiRender;
        }

        private void Awake()
        {
            var imgui = GetComponent<DearImGui>();
        }


        private void OnImGuiRender()
        {
            foreach (var win in editorWindows) 
            {
                win.OnImGuiRenderer();
            }
        }
    }
}