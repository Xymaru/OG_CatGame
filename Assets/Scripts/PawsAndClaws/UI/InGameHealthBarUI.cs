using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class InGameHealthBarUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider easeSlider;
        [SerializeField] private float waitTimeToFill = 1f;
        [SerializeField] private float fillSpeed = 1.5f;

        private Coroutine _easeBarCoroutine = null;
        public void UpdateBar(float value, float maxValue)
        {
            healthSlider.maxValue = maxValue;
            healthSlider.value = value;

            if (_easeBarCoroutine != null)
            {
                StopCoroutine(_easeBarCoroutine);
            }

            _easeBarCoroutine = StartCoroutine(EaseBarCoroutine(value, maxValue));
        }

        private IEnumerator EaseBarCoroutine(float value, float maxValue)
        {
            easeSlider.maxValue = maxValue;
            float timer = 0f;
            yield return new WaitForSeconds(waitTimeToFill);

            float hFraction = value / maxValue;
            while (easeSlider.value > hFraction)
            {
                timer += Time.deltaTime;
                float percentComplete = timer / fillSpeed;
                percentComplete *= percentComplete;
                easeSlider.value = Mathf.Lerp(easeSlider.value, value, percentComplete);
                yield return null;
            }
            _easeBarCoroutine = null;
        }
    }
}
