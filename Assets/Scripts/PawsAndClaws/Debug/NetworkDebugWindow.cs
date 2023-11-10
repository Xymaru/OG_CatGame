using ImGuiNET;
using PawsAndClaws.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class NetworkDebugWindow : DebugWindow
    {

      

        public override void OnImGuiRenderer()
        {
            ImGui.Begin("Network");


            ImGui.Text($"Protocol: {NetworkData.ProtocolType}");
            ImGui.Text($"EndPoint: {NetworkData.ServerEndPoint}");


            ImGui.End();
        }
    }
}