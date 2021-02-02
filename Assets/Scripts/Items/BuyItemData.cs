using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using Assets.Scripts.UI.SelectPopup;
using UnityEngine;

namespace Assets.Scripts.Items {

    public class BuyItemData : IForSelectPanel {

        public BuyItemData(ItemData item)
            => itemData = item;

        public ItemData itemData;

        public Sprite img => itemData.img;

        public string title => itemData.title;

        public string description => itemData.description;

        public string selectText => TranslateReader.GetTranslate("item_cost", new Placeholder("cost", itemData.cost.ToString()));

        public ObservableVal onChange => itemData.onChange;

        public bool isEnableToSelect => GameMng.current.playerData.money >= itemData.cost;

        public void Select() {

            GameMng.current.playerData.money.val -= itemData.cost;
            GameMng.current.playerData.itemsContainer.AddPrefab(itemData);

        }
    }
}
