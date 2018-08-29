using System;
using DG.Tweening;
using UnityEngine;

namespace Controls
{
    public class PieControl : MonoBehaviour
    {
        [SerializeField] private float tweenDuration = 0.3f;

        private Sequence sequence;

        private float weight = 1f;

        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
                UpdateScale(value);
            }
        }

        private void OnDestroy()
        {
            DisposeTween();
        }

        public void DisposeTween()
        {
            sequence?.Kill();
            sequence = null;
        }

        private void UpdateScale(float scale)
        {
            transform.localScale = new Vector3(scale, 1f, scale);
        }

        public void Move(float height, Vector3 point, TweenCallback completionHandler)
        {
            DisposeTween();

            sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(height, tweenDuration));
            sequence.Append(transform.DOMove(new Vector3(point.x, height, point.z), tweenDuration));
            sequence.Append(transform.DOMove(point, tweenDuration));
            sequence.OnComplete(completionHandler);
        }
    }
}