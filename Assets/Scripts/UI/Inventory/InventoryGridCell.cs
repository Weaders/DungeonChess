using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory {

    [RequireComponent(typeof(MoveItemCell))]
    public class InventoryGridCell : MonoBehaviour {

        [SerializeField]
        private MoveItemCell moveItemCell;

        private ItemData itemData;

        private ItemsContainer itemsContainer;

        private int indexInItems;

        private void Awake() {

            moveItemCell.onChangeItem.AddListener((oldVal, newVal) => {

                if (oldVal != null) {

                    itemsContainer.Set(null, indexInItems);
                    itemData = null;

                }

                if (newVal != null) {

                    itemData = GameMng.current.moveItemFactory.Get<ItemData>(newVal);
                    itemsContainer.Set(itemData, indexInItems);
                    
                }

            });

        }

        public void SetItemsContainer(ItemsContainer itemContainer, int index) {

            itemsContainer = itemContainer;
            indexInItems = index;

            itemsContainer.onAdd.AddSubscription(Observable.OrderVal.UIUpdate, (changeData) => {
                
                if (changeData.index == index) {
                    InitWithItem(changeData.data);
                }

            });

        }

        public void InitWithItem(ItemData item) {

            var mv = GameMng.current.moveItemFactory.CreateOrGet(item);
            moveItemCell.InitWithItem(mv);

            itemData = item;

        }

        public void SetItem(ItemData item) {

            if (item != null) {

                var mv = GameMng.current.moveItemFactory.CreateOrGet(item);
                mv.PlaceIn(moveItemCell);

            }

            itemData = item;

        }

        public ItemData GetItem() => itemData;

        private void OnDestroy() {
            if (itemData != null)
                GameMng.current.moveItemFactory.Remove(itemData);
        }

        public bool IsThereItem() => GetItem() != null;

    }
}
