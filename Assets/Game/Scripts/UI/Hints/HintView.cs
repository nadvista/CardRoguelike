using TMPro;
using UnityEngine;

namespace Ui.Hint
{
    public class HintView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI label;

        private void Awake()
        {
            Hide();
        }

        public void Show(string text)
        {
            label.text = text;
            Show();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
