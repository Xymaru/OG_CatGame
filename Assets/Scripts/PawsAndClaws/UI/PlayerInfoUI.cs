using System;
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
        [Header("UI elements")]
        [SerializeField] private Image playerImage;
        [SerializeField] private TMPro.TextMeshProUGUI respawnCooldownTimer;
        [SerializeField] private AbilitiesUI abilitiesUI;
        [SerializeField] private RegenBarUI healthBar;
        [SerializeField] private RegenBarUI manaBar;
        [SerializeField] private BarUI levelBar;
        [SerializeField] private TMPro.TextMeshProUGUI levelText;
        [SerializeField] private StatsUI statsUI;
        [Header("Other")]
        [SerializeField] private Material grayScaleMaterial;
        
        private Coroutine _respawnCoroutine;
        private void OnEnable()
        {
            manager.onHealthChange          += healthBar.UpdateBar;
            manager.onHealthRegenChange     += healthBar.UpdateRegen;
            manager.onManaChange            += manaBar.UpdateBar;
            manager.onManaRegenChange       += manaBar.UpdateRegen;
            manager.onExpChange             += levelBar.UpdateBar;
            manager.onPlayerDied            += OnPlayerDied;
            manager.onPlayerSpawn           += OnPlayerSpawn;
            manager.onLevelUp               += UpdateLevelText;
            manager.onStatsChanged          += statsUI.UpdateStats;
        }
        private void OnDisable()
        {
            manager.onHealthChange          -= healthBar.UpdateBar;
            manager.onHealthRegenChange     -= healthBar.UpdateRegen;
            manager.onManaChange            -= manaBar.UpdateBar;
            manager.onManaRegenChange       -= manaBar.UpdateRegen;
            manager.onExpChange             -= levelBar.UpdateBar;
            manager.onPlayerDied            -= OnPlayerDied;
            manager.onPlayerSpawn           -= OnPlayerSpawn;
            manager.onLevelUp               -= UpdateLevelText;
            manager.onStatsChanged          -= statsUI.UpdateStats;
        }
        
        private void Awake()
        {
            playerImage.sprite = manager.characterData.sprite;
            respawnCooldownTimer.text = "";
        }

        private void OnPlayerDied(float timer)
        {
            playerImage.material = grayScaleMaterial;
            _respawnCoroutine ??= StartCoroutine(RespawnTimerCoroutine(timer));
        }

        private void OnPlayerSpawn()
        {
            playerImage.material = null;
            respawnCooldownTimer.text = "";
            StopCoroutine(_respawnCoroutine);
            _respawnCoroutine = null;
        }
        private IEnumerator RespawnTimerCoroutine(float time)
        {
            while (time > 0)
            {
                time -= Time.deltaTime;
                respawnCooldownTimer.text = $"{(int)time}";
                yield return null;
            }
        }

        void UpdateLevelText(int level)
        {
            levelText.text = $"{level}";
        }
        
    }
}