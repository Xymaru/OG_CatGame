using TMPro;
using UnityEngine;

namespace PawsAndClaws.UI
{
    public class InGamePlayerHealthBarUI : InGameHealthBarUI
    {
        [SerializeField] private TextMeshProUGUI userText;

        public void UpdateName(string userName)
        {
            userText.text = userName;
        }
    }
}
