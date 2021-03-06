﻿using System.Linq;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using Assets.Scripts.UI.DragAndDrop;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Items {

    public abstract class ItemData : MonoBehaviour, IForMoveItem, IForSelectPanel, ISource {

        [SerializeField]
        protected string id;

        [SerializeField]
        private string _title;

        [SerializeField]
        private string _description;

        public int cost;

        public CharacterData owner { get; protected set; }

        public Sprite icon;

        public string title => TranslateReader.GetTranslate(_title, GetPlaceholders(owner));

        public string description => TranslateReader.GetTranslate(_description, GetPlaceholders(owner));

        public Sprite img => icon;

        public ObservableVal onChange => new ObservableVal();

        public string selectText => TranslateReader.GetTranslate("add_in_player_invent");

        public bool isEnableToSelect => true;

        public UnityEvent onDestroy { get; } = new UnityEvent();

        public virtual ObservableVal<int> count => null;

        public void Equip(CharacterData characterData) {
            owner = characterData;
            Equip();
        }

        public void DeEquip() {

            OnDeEquip();
            owner = null;

        }

        protected abstract void Equip();

        protected abstract void OnDeEquip();

        public void ClickHandle(MoveItem moveItem) {
            GameMng.current.itemInfoPanel.SetItemData(this);
        }

        protected virtual Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var place = this.GetPlaceholdersFromAttrs(descriptionFor);
            return place.ToArray();

        }

        public void Select() {
            GameMng.current.playerData.itemsContainer.AddPrefab(this);
        }

        public string GetId() => id;

        public virtual bool IsNeedMoveBack() {
            return false;
        }

        private void OnDestroy() {
            onDestroy.Invoke();
        }
    }

}
