using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.Events;
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

        [SerializeField]
        private Transform fallBackContainer;

        [SerializeField]
        private Button fallBackbtnPrefab;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// set items
        /// </summary>
        /// <param name="items">
        /// I will be butn in the hell, for this code :D
        /// </param>
        public void SetItems((IForSelectPanel, IForSelectPanel, IForSelectPanel) items, Action<IForSelectPanel> onSelect = null, IEnumerable<FallBackBtn> btns = null) {

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

            if (btns == null)
                btns = new[] { new FallBackBtn(TranslateReader.GetTranslate("hide"), Hide) };

            foreach (Transform tr in fallBackContainer)
                Destroy(tr.gameObject);

            foreach (var btnData in btns) {

                var btn = Instantiate(fallBackbtnPrefab, fallBackContainer);

                btn.GetComponentInChildren<Text>().text = btnData.text;
                btn.onClick.AddListener(() => btnData.unityAction());

            }

        }

        public string titleText {
            get => title.text;
            set => title.text = value;
        }

        public void Show() {
            canvasGroup.Show();
        }

        public void Hide() {
            canvasGroup.Hide();
            GameMng.current.topSidePanelUI.stateTopBtn = TopSidePanel.StateTopBtn.ShowMenu;
        }

        public bool IsShowed
            => canvasGroup.IsShowed();

        public class FallBackBtn {

            public FallBackBtn(string _text, UnityAction action) {
                text = _text;
                unityAction = action;
            }

            public FallBackBtn(TranslateReader translateReader) {
                this.translateReader = translateReader;
            }

            public string text;
            public UnityAction unityAction;
            private TranslateReader translateReader;
        }

    }
}
