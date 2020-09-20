using Assets.Scripts.Character;
using Assets.Scripts.Translate;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Items {

    public abstract class ItemData : MonoBehaviour, IForMoveItem {

        [SerializeField]
        private string id;

        [SerializeField]
        private string _title;

        [SerializeField]
        private string _description;

        public Sprite icon;

        public string title => TranslateReader.GetTranslate(_title);

        public string description => TranslateReader.GetTranslate(_description);

        public abstract void Equip(CharacterData characterData);

        public abstract void DeEquip(CharacterData characterData);

        public void ClickHandle(MoveItem moveItem) {
            GameMng.current.itemInfoPanel.SetItemData(this);
        }
    }
}
