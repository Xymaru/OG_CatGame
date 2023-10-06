using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.UI
{
    public class RegenBarUI : BarUI
    {
        [SerializeField] private TMPro.TextMeshProUGUI regenText;

        public void UpdateRegen(float value)
        {
            regenText.text = $"+{value}";
        }
    }
}