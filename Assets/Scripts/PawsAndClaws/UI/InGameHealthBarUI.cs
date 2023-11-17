using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class InGameHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Image frontImage;
        [SerializeField] private Image backImage;
        
        [SerializeField] private float fillSpeed = 2;

        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color healColor = Color.green;

        private float _value;
        private float _maxValue;
        private float _lerpTimer;

        public void UpdateBar(float value, float maxValue)
        {
            this._value = value;
            this._maxValue = maxValue;
            _lerpTimer = 0;
        }


        private void Update()
        {
            UpdateHealthUI();
        }

        private void UpdateHealthUI()
        {
            var fillF = frontImage.fillAmount;
            var fillB = backImage.fillAmount;
            var hFraction = _value / _maxValue;
            if(fillB > hFraction) 
            {
                frontImage.fillAmount = hFraction;
                backImage.color = damageColor;
                _lerpTimer += Time.deltaTime;
                var percentComplete = _lerpTimer / fillSpeed;
                percentComplete *= percentComplete;
                backImage.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
            }
            if(fillF < hFraction)
            {
                backImage.fillAmount = hFraction;
                backImage.color = healColor;
                _lerpTimer += Time.deltaTime;
                float percentComplete = _lerpTimer / fillSpeed;
                percentComplete *= percentComplete;
                frontImage.fillAmount = Mathf.Lerp(fillF, backImage.fillAmount, percentComplete);
            }
        }
    }
}
