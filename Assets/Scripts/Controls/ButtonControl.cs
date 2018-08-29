using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
    [RequireComponent(typeof(Button))]
    public class ButtonControl : MonoBehaviour
    {
        [SerializeField] private Text label;

        private Button button;

        public string Label
        {
            get { return label.text; }
            set { label.text = value; }
        }

        public Button.ButtonClickedEvent OnClick => button.onClick;

        private void Awake()
        {
            this.Label = ""; 
            button = GetComponent<Button>();
        }
    }
}
