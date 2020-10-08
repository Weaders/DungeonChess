using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI.SelectPopup {

    [RequireComponent(typeof(CanvasGroup))]
    public class SelectPanel : MonoBehaviour {

        [SerializeField]
        private SelectPanelItem[] selectPanelItems;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// set items
        /// </summary>
        /// <param name="items">
        /// I will be butn in the hell, for this code :D
        /// </param>
        public void SetItems((IForSelectPanel, IForSelectPanel, IForSelectPanel) items) {

            selectPanelItems[0].SetItem(items.Item1);
            selectPanelItems[1].SetItem(items.Item2);
            selectPanelItems[2].SetItem(items.Item3);

        }

        public void Show() {
            canvasGroup.Show();
        }

        public void Hide() {
            canvasGroup.Hide();
        }

    }
}
