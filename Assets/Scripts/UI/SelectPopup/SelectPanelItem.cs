using System;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectPopup {

    public interface IForSelectPanel {

        Sprite img { get; }

        string title { get; }

        string description { get; }

        string selectText { get; }

        ObservableVal onChange { get; }

        bool isEnableToSelect { get; }

        void Select();

    }

    public class SelectPanelItem : MonoBehaviour {

        [SerializeField]
        private Image img;

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text description;

        [SerializeField]
        private Text btnText;

        [SerializeField]
        private Button button;

        private Action<IForSelectPanel> _onSelect;

        private IForSelectPanel _item;

        public void Select() {

            var forSelect = _onSelect;

            GameMng.current.selectPanel.Hide();
            _item.Select();
            forSelect?.Invoke(_item);

        }

        public void SetItem(IForSelectPanel item, Action<IForSelectPanel> onSelect = null) {

            img.sprite = item.img;
            title.text = item.title;
            description.text = item.description;
            btnText.text = item.selectText;

            button.interactable = item.isEnableToSelect;

            _item = item;
            _onSelect = onSelect;

        }

    }
}
