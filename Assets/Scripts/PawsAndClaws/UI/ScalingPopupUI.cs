using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PawsAndClaws.UI
{
    public class ScalingPopupUI : MonoBehaviour
    {
        public bool open = false;
        [SerializeField] private LeanTweenType easeType;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Vector3 endScale = new Vector3(1, 1, 1);

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        public void Open()
        {
            open = true;
            if (easeType == LeanTweenType.animationCurve)
            {
                LeanTween.scale(gameObject, endScale, duration).setEase(curve);
            }
            else
            {
                LeanTween.scale(gameObject, endScale, duration).setEase(easeType);
            }
        }

        public void Close()
        {
            open = false;
            if (easeType == LeanTweenType.animationCurve)
            {
                LeanTween.scale(gameObject, Vector3.zero, duration).setEase(curve);
            }
            else
            {
                LeanTween.scale(gameObject, Vector3.zero, duration).setEase(easeType);
            }
        }
    }
}
