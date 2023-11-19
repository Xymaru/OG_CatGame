using System.Collections;
using System.Collections.Generic;
using PawsAndClaws.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PawsAndClaws.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _optionsMenu;

        private void Awake()
        {
            _mainMenu.SetActive(true);
            _optionsMenu.SetActive(false);
            
            var userName = PlayerPrefs.GetString("user_name");
            if (userName != "")
            {
                GameConstants.UserName = userName;
            }
        }


        public void PlayGame()
        {
            SceneManager.LoadScene("PlayMenu");
        }

        public void ShowOptions()
        {
            _mainMenu.SetActive(false);
            _optionsMenu.SetActive(true);
        }

        public void HideOptions()
        {
            _mainMenu.SetActive(true);
            _optionsMenu.SetActive(false);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        private void OnDestroy()
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            PlayerPrefs.SetString("user_name", GameConstants.UserName);
        }
    }
}