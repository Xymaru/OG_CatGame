using PawsAndClaws.Game;
using UnityEngine;

namespace PawsAndClaws.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private SlidingMenuUI playMenu;
        [SerializeField] private SlidingMenuUI optionsMenu;
        [SerializeField] private ScalingPopupUI exitMenu;
        
        private void Awake()
        {
            var userName = PlayerPrefs.GetString("user_name");
            if (userName != "")
            {
                GameConstants.UserName = userName;
            }
        }


        public void PlayGame()
        {
            if (playMenu.open)
            {
                playMenu.Close();
                
            }
            else
            {
                playMenu.Open();
                optionsMenu.Close();
                exitMenu.Close();
            }
        }

        public void ShowOptions()
        {
            if (optionsMenu.open)
            {
                optionsMenu.Close();
            }
            else
            {
                optionsMenu.Open();
                playMenu.Close();
                exitMenu.Close();
            }
        }

        public void ShowExit()
        {
            if (exitMenu.open)
            {
                exitMenu.Close();
            }
            else
            {
                exitMenu.Open();
                playMenu.Close();
                optionsMenu.Close();
            }
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