using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PawsAndClaws.Player;

namespace PawsAndClaws.UI
{
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;
        [SerializeField] private TextMeshProUGUI shieldText;
        [SerializeField] private TextMeshProUGUI attackSpeedText;
        [SerializeField] private TextMeshProUGUI rangeText;

        public void UpdateStats(CharacterStats stats)
        {
            damageText.text = $"Damage {stats.Damage}";
            shieldText.text = $"Shield {stats.Shield}";
            attackSpeedText.text = $"Attack Speed {stats.AttackSpeed}";
            rangeText.text = $"Range {stats.Range}";
        }
    }
}