using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
    [RequireComponent(typeof(Text))]
    public class TextControl : MonoBehaviour
    {
        [SerializeField] private string formatter = "{0:0.00}";

        private Text text;

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        public void SetValue(float value)
        {
            text.text = string.Format(formatter, value);
        }
    }
}
