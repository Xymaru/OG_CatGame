using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PawsAndClaws.UI
{
    public struct Notification
    {
        public readonly string Title;
        public readonly string Subtitle;
        public readonly UnityAction  ButtonAction;
        public readonly float Duration;

        public Notification(string title, float duration)
        {
            Title = title;
            Subtitle = "";
            ButtonAction = null;
            Duration = duration;
        }
        
        public Notification(string title, string subtitle, UnityAction buttonAction)
        {
            Title = title;
            Subtitle = subtitle;
            ButtonAction = buttonAction;
            Duration = -1f;
        }
    }
    
    public class NotificationsUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI titleText;
        [SerializeField] private TMPro.TextMeshProUGUI subtitleText;
        [SerializeField] private Button button;
        
        private readonly Queue<Notification> _pendingNotifications = new Queue<Notification>();
        private Notification _currentNotification;
        
        public static NotificationsUI Instance { get; private set; }

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;

            StartCoroutine(PopNotificationLoop());
        }

        public void PushNotification(Notification notification)
        {
            _pendingNotifications.Enqueue(notification);
        }

        private IEnumerator PopNotificationLoop()
        {
            while (true)
            {
                transform.localScale = Vector3.zero;
                
                yield return new WaitUntil(() => _pendingNotifications.Count != 0);
                
                _currentNotification = _pendingNotifications.Dequeue();

                titleText.text = _currentNotification.Title;
                subtitleText.text = _currentNotification.Subtitle;


                transform.localScale = Vector3.one;
                
                // If the notification has a value of -1 for the duration it means that we should wait for the button event to pop
                if (_currentNotification.Duration > 0f)
                {
                    button.gameObject.SetActive(false);
                    yield return new WaitForSeconds(_currentNotification.Duration);
                }
                else
                {
                    button.gameObject.SetActive(true);
                    button.onClick.AddListener(ContinueCoroutine);
                    button.onClick.AddListener(_currentNotification.ButtonAction);
                    yield break;
                }
            }
        }

        private void ContinueCoroutine()
        {
            StartCoroutine(PopNotificationLoop());
        }
    }
}
