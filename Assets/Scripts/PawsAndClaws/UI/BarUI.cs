using UnityEngine;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class BarUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMPro.TextMeshProUGUI text;
        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public void UpdateBar(float value, float maxValue)
        {
            slider.value = value / maxValue;
            slider.maxValue = maxValue;

            text.text = $"{value} / {maxValue}";
        }
    }
}