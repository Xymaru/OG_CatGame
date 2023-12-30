
using System;
using PawsAndClaws.Game;
using PawsAndClaws.Player;
using UnityEngine;

namespace PawsAndClaws.Debugging
{
    public class PlayerInfoWindow : DebugWindow
    {
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 200, Screen.height - 100, 200, 100));
            if (GUILayout.Button("Lose Game"))
            {
                GameManager.Instance.LoseGame(GameManager.Instance.PlayerTeam);
            }

            if (GUILayout.Button("Win Game"))
            {
                GameManager.Instance.LoseGame(GameManager.Instance.oppositeTeam);
            }
            
            GUILayout.EndArea();
        }
    }
}