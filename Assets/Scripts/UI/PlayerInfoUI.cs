using PawsAndClaws.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class PlayerInfoUI : MonoBehaviour
    {
        public PlayerManager manager;

        public Image playerImage;
        public AbilitiesUI abilitiesUI;
        public RegenBarUI healthBar;
        public RegenBarUI manaBar;
        public BarUI levelBar;

        private void OnEnable()
        {
            manager.onHealthChange          += healthBar.UpdateBar;
            manager.onHealthRegenChange     += healthBar.UpdateRegen;
            manager.onManaChange            += manaBar.UpdateBar;
            manager.onManaRegenChange       += manaBar.UpdateRegen;
            manager.onExpChange             += levelBar.UpdateBar;
        }

        private void OnDisable()
        {
            manager.onHealthChange          -= healthBar.UpdateBar;
            manager.onHealthRegenChange     -= healthBar.UpdateRegen;
            manager.onManaChange            -= manaBar.UpdateBar;
            manager.onManaRegenChange       -= manaBar.UpdateRegen;
            manager.onExpChange             -= levelBar.UpdateBar;
        }
    }
}