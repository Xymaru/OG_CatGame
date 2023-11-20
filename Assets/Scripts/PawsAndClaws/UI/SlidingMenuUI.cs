using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PawsAndClaws.UI
{
    public class SlidingMenuUI : MonoBehaviour
    {
        public bool open = false;
        [SerializeField] private LeanTweenType easeType;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private float endPos = 0f;
        private float _startPos;

        private void Start()
        {
            _startPos = transform.localPosition.x;
        }

        public void Open()
        {
            open = true;

            if (easeType == LeanTweenType.animationCurve)
            {
                LeanTween.moveLocalX(gameObject, endPos, duration).setEase(curve);
            }
            else
            {
                LeanTween.moveLocalX(gameObject, endPos, duration).setEase(easeType);
            }
            
        }
        public void Close()
        {
            open = false;
            if (easeType == LeanTweenType.animationCurve)
            {
                LeanTween.moveLocalX(gameObject, _startPos, duration).setEase(curve);
            }
            else
            {
                LeanTween.moveLocalX(gameObject, _startPos, duration).setEase(easeType);
            }
        }
    }
}