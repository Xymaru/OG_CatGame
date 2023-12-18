using System;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class NetworkDebugWindow : MonoBehaviour
    {
        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 100, 90), "Network");
            
            GUI.Label(new Rect(20, 20, 70, 20), "Packets sent");
            GUI.Label(new Rect(20, 50, 70, 20), "Packets received");
            
            
        }
    }
}