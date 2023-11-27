using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class AbilityHolderUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _cooldownText;
        [SerializeField] private Image _cooldownImage;
        private float cooldown = 0;
        private float time;
        private bool startCooldown = false;
        private void Start()
        {
            _cooldownText.gameObject.SetActive(false);
            _cooldownImage.gameObject.SetActive(false);
        }

        public void StartCooldown(float cooldown)
        {
            time = this.cooldown = cooldown;
            startCooldown = true;

            _cooldownText.gameObject.SetActive(true);
            _cooldownImage.gameObject.SetActive(true);
            _cooldownImage.fillAmount = 0;
            _cooldownText.text = cooldown.ToString("F0");
        }

        private void Update()
        {
            if (!startCooldown)
                return;

            time -= Time.deltaTime;
            _cooldownText.text = cooldown.ToString("F0");
            _cooldownImage.fillAmount = Mathf.Clamp(time, 0, cooldown);
        }
    }
}