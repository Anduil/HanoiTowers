using System;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class TimeScaleController : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] [Range(0.1f, 10f)] private float minTimeScale = 0.1f;
        [SerializeField] [Range(0.1f, 100f)] private float maxTimeScale = 100f;

        private void OnValidate()
        {
            if (minTimeScale > maxTimeScale)
            {
                Debug.LogError(new ArgumentOutOfRangeException());
            }
        }

        private void Start()
        {
            // Validate just for case.
            if (minTimeScale > maxTimeScale)
            {
                minTimeScale = 0.1f;
                maxTimeScale = 10f;
            }

            slider.maxValue = maxTimeScale;
            slider.minValue = minTimeScale;

            Time.timeScale = 1f;
            slider.value = Time.timeScale;

            slider.onValueChanged.AddListener(SetTimeScale);
        }

        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(SetTimeScale);
        }

        /// <summary>
        /// Set time scale.
        /// </summary>
        /// <param name="value">Value between 0.0f [including] and 1.0f [including].</param>
        public void SetTimeScale(float value)
        {
            Time.timeScale = value;
        }
    }
}
