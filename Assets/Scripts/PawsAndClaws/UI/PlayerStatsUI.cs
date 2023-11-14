using System;
using PawsAndClaws.Game;
using UnityEngine;

namespace PawsAndClaws.UI
{
    public class PlayerStatsUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI timeText;


        private void Update()
        {
            timeText.text = GameManager.Instance.MatchTimeString;
        }
    }
}
