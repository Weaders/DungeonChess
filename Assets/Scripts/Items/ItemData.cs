using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using Assets.Scripts.UI;
using Assets.Scripts.UI.DragAndDrop;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;

namespace Assets.Scripts.Items {

    public abstract class ItemData : MonoBehaviour, IForMoveItem, IForSelectPanel {

        [SerializeField]
        private string id;

        [SerializeField]
        private string _title;

        [SerializeField]
        private string _description;

        public Sprite icon;

        public string title => TranslateReader.GetTranslate(_title);

        public string description => TranslateReader.GetTranslate(_description);

        public Sprite img => icon;

        public ObservableVal onChange => new ObservableVal();

        public abstract void Equip(CharacterData characterData);

        public abstract void DeEquip(CharacterData characterData);

        public void ClickHandle(MoveItem moveItem) {
            GameMng.current.itemInfoPanel.SetItemData(this);
        }

        public void Select() {
            GameMng.current.playerData.itemsContainer.AddPrefab(this);
        }
    }
}
