using Assets.Scripts.Items;
using Assets.Scripts.Observable;
using Assets.Scripts.UI.DragAndDrop;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory {

    [RequireComponent(typeof(MoveItemCell))]
    public class InventoryGridCell : MonoBehaviour {

        [SerializeField]
        private MoveItemCell moveItemCell;

        private ItemData itemData;

        private ItemsContainer itemsContainer;

        private int indexInItems;

        private bool _isBelongsToUser;

        public bool isBelongsToUser {
            get => _isBelongsToUser;
            set {

                _isBelongsToUser = value;

                if (!_isBelongsToUser) {
                    moveItemCell.state = MoveItemCell.State.Locked;
                }

            }
        }

        private void Awake() {

            moveItemCell.onChangeItem.AddListener((oldVal, newVal) => {

                if (oldVal != newVal && newVal != itemData) 
                {

                    if (oldVal != null) {

                        itemsContainer[indexInItems] = null;
                        itemData = null;

                    }

                    if (newVal != null) {

                        itemData = GameMng.current.moveItemFactory.Get<ItemData>(newVal);
                        itemsContainer[indexInItems] = itemData;

                    }

                }

            });

        }

        public void SetItemsContainer(ItemsContainer itemContainer, int index) {

            if (itemsContainer != null) {
                itemsContainer.onSet.RemoveSubscription(OnItemsContainerChange);
            }

            itemsContainer = itemContainer;
            indexInItems = index;

            itemsContainer.onSet.AddSubscription(OrderVal.UIUpdate, OnItemsContainerChange);

            if (itemContainer[index] != null)
                PlaceItem(itemsContainer[index], false);

        }

        public void OnItemsContainerChange(SetEnumerableItemEvent<ItemData> changeData) {

            if (changeData.index == indexInItems && changeData.data != itemData) {
                PlaceItem(changeData.data);
            }

        }

        private void PlaceItem(ItemData itemData, bool fireTriggers = true) {

            MoveItem mv = null;

            if (itemData != null) {
                mv = GameMng.current.moveItemFactory.CreateOrGet(MoveItemFactory.ReasonCreate.Inventory, itemData);
            }

            GameMng.current.moveItemSystem.PlaceItem(mv, moveItemCell, fireTriggers);

        }

        public ItemData GetItem() => itemData;

        public bool IsThereItem() => GetItem() != null;

        public bool IsCanUserPlaceItem() => !IsThereItem() && isBelongsToUser;

    }
}
