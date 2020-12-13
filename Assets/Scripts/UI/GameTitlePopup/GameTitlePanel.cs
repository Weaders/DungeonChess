using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameTitlePopup {

    [RequireComponent(typeof(CanvasGroup))]
    public class GameTitlePanel : MonoBehaviour {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetData(SetDataMsg setDataMsg) {
            titleText.text = setDataMsg.text;
        }

        public void Show() {
            canvasGroup.Show();
        }

        public void ShowPopup(string title, float delay = 2f) {

            titleText.text = title;
            Invoke("HidePopup", delay);
            Show();

        }

        public void HidePopup() {
            canvasGroup.Hide();
        }

        public class SetDataMsg {
            public string text;
        }

    }
}
