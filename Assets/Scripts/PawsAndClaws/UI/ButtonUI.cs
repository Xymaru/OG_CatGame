using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public class ButtonUI : MonoBehaviour
    {
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip unHoverSound;
        
        private AudioSource _audioSource;
        private Button _button;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(PlayClick);
        }

        private void PlayClick()
        {
            _audioSource.PlayOneShot(clickSound);
        }

        public void PlayHover()
        {
            _audioSource.PlayOneShot(hoverSound);
        }

        public void PlayUnHover()
        {
            _audioSource.PlayOneShot(unHoverSound);
        }
    }
}
