using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PawsAndClaws.Scenes
{
    public class SceneTransitionManager : MonoBehaviour
    {
        public static Action<int> TransitionTo;

        [SerializeField] private float duration = 0.5f;
        [SerializeField] private LeanTweenType easeType;
        private Image _image;
        private void OnEnable()
        {
            TransitionTo += ChangeLevelTo;
        }

        private void OnDisable()
        {
            TransitionTo -= ChangeLevelTo;
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.fillAmount = 1;
        }

        private void Start()
        {
            StartTransition();
        }

        private void StartTransition()
        {
            LeanTween.value(gameObject, (float value) =>
            {
                _image.fillAmount = value;
            }, 1, 0, duration).setEase(easeType);
        }
        
        private void ChangeLevelTo(int levelId)
        {
            LeanTween.value(gameObject, (float value) =>
            {
                _image.fillAmount = value;
            }, 0, 1, duration).setEase(easeType).setOnComplete(() =>
            {
                SceneManager.LoadSceneAsync(levelId);
            });
        }
    }
}
