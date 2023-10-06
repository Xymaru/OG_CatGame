using UnityEngine;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class BarUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMPro.TextMeshProUGUI text;
        public void UpdateBar(float value, float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = value;
            text.text = $"{value} / {maxValue}";
        }
    }
}