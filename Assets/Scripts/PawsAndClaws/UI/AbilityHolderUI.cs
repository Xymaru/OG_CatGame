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
        [SerializeField] private Button _button;

        private float cooldown = 0f;
        private float time = 0f;
        private bool startCooldown = false;

        private void Start()
        {
            _cooldownText.gameObject.SetActive(false);
            _cooldownImage.gameObject.SetActive(false);
        }

        public void SetImage(Sprite image)
        {
            _button.image.sprite = image;
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
            _cooldownText.text = time.ToString("F0");
            _cooldownImage.fillAmount = time / cooldown;

            // Reset the timers
            if(time <= 0f)
            {
                startCooldown = false;
                time = 0f;
                cooldown = 0f;
                _cooldownText.gameObject.SetActive(false);
                _cooldownImage.gameObject.SetActive(false);
                _cooldownImage.fillAmount = 0;
            }
        }
    }
}