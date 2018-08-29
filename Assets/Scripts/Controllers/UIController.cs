using System;
using Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Controllers
{
    // ReSharper disable once InconsistentNaming
    public class UIController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private Slider slider;
        [SerializeField] private InputField inputField;
        [SerializeField] private ButtonControl button;
        [SerializeField] private Transform finishPanel;

        public UnityEvent OnClick => button.OnClick;

        public int GetCountPies()
        {
            int count;
            var parsed = int.TryParse(inputField.text, out count); 
            return parsed ? count : 0;
        }

        public void SetState(States state)
        {
            switch (state)
            {
                case States.Playing:
                    inputField.enabled = false;
                    button.Label = "Очистить";
                    finishPanel.gameObject.SetActive(false);
                    break;
                case States.Stopped:
                    inputField.enabled = true;
                    button.Label = "Решить";
                    finishPanel.gameObject.SetActive(false);
                    break;
                case States.Finish:
                    inputField.enabled = false;
                    button.Label = "Очистить";
                    finishPanel.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public enum States
        {
            Playing,
            Stopped,
            Finish
        }
    }
}
