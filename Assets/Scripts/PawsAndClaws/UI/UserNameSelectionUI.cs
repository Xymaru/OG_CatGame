using System;
using PawsAndClaws.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class UserNameSelectionUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private UserNameUI userNameUI;
        [SerializeField] private ScalingPopupUI popup;
        public void Start()
        {
            if (GameConstants.UserNameSet)
            {
                Destroy(gameObject);
            }
            button.onClick.AddListener(SetUserName);   
            popup.Open();
        }

        private void SetUserName()
        {
            userNameUI.SetUserName(inputField.text);
            userNameUI.OnUserNameUnfocus();
            MainMenuUI.SaveSettings();
            popup.Close(OnUserFade);
        }

        private void OnUserFade()
        {
            Destroy(gameObject);
        }
    }
}
