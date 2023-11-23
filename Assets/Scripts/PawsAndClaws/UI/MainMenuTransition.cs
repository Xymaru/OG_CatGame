using PawsAndClaws.Scenes;
using UnityEngine;

namespace PawsAndClaws.UI
{
    public class MainMenuTransition : SceneTransitionManager
    {
        [SerializeField] private GameObject mainMenu;
        protected override void StartTransition()
        {
            mainMenu.SetActive(false);
            LeanTween.value(gameObject, (float value) =>
            {
                _image.fillAmount = value;
            }, 1, 0, duration).setEase(easeType).setOnComplete(EnableScene);
        }

        private void EnableScene()
        {
            mainMenu.SetActive(true);
        }
    }
}
