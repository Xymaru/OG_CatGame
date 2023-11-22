using System;
using PawsAndClaws.Game;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class UserNameUI : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        [SerializeField] private GameObject inputField;
        [SerializeField] private TextMeshProUGUI userText;

        private TMP_InputField _input;
        private void Start()
        {
            userText.text = GameConstants.UserName;
            _input = inputField.GetComponent<TMP_InputField>();
            
            button.SetActive(true);
            inputField.SetActive(false);
        }

        public void SetUserName(string userName)
        {
            if (userName == "") return;

            GameConstants.UserName = userName;
        }
        
        public void OnUserNameClicked()
        {
            inputField.SetActive(true);
            button.SetActive(false);
            _input.Select();
            _input.ActivateInputField();
        }

        public void OnUserNameUnfocus()
        {
            userText.text = GameConstants.UserName;
            button.SetActive(true);
            inputField.SetActive(false);
        }
        
    }
}
