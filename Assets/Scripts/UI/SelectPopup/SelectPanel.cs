using System;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectPopup {

    [RequireComponent(typeof(CanvasGroup))]
    public class SelectPanel : MonoBehaviour {

        [SerializeField]
        private SelectPanelItem[] selectPanelItems;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Text title;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// set items
        /// </summary>
        /// <param name="items">
        /// I will be butn in the hell, for this code :D
        /// </param>
        public void SetItems((IForSelectPanel, IForSelectPanel, IForSelectPanel) items, Action<IForSelectPanel> onSelect = null) {

            if (items.Item1 == null)
                selectPanelItems[0].gameObject.SetActive(false);
            else {
                selectPanelItems[0].gameObject.SetActive(true);
                selectPanelItems[0].SetItem(items.Item1, onSelect);
            }
                

            if (items.Item2 == null)
                selectPanelItems[1].gameObject.SetActive(false);
            else {
                selectPanelItems[1].gameObject.SetActive(true);
                selectPanelItems[1].SetItem(items.Item2, onSelect);
            }
                

            if (items.Item3 == null)
                selectPanelItems[2].gameObject.SetActive(false);
            else {
                selectPanelItems[2].gameObject.SetActive(true);
                selectPanelItems[2].SetItem(items.Item3, onSelect);
            }
                

        }

        public string titleText {
            get => title.text;
            set => title.text = value;
        }

        public void Show() {
            canvasGroup.Show();
            //GameMng.current.topSidePanelUI.stateTopBtn = TopSidePanel.StateTopBtn.Start;
        }

        public void Hide() {
            canvasGroup.Hide();
            GameMng.current.topSidePanelUI.stateTopBtn = TopSidePanel.StateTopBtn.ShowMenu;
        }

        public bool IsShowed
            => canvasGroup.IsShowed();

    }
}
