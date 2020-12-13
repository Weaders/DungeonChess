using System;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SelectPopup {

    public interface IForSelectPanel {

        Sprite img { get; }

        string title { get; }

        string description { get; }

        ObservableVal onChange { get; }

        void Select();

    }

    public class SelectPanelItem : MonoBehaviour {

        [SerializeField]
        private Image img;

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text description;

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

            _item = item;
            _onSelect = onSelect;

        }

    }
}
