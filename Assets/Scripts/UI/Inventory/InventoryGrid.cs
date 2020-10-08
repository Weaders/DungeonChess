using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.UI.Inventory {

    public class InventoryGrid : MonoBehaviour {

        [SerializeField]
        private InventoryGridCell inventoryGridCellPrefab;

        private ItemsContainer _itemsContainer;

        private InventoryGridCell[] inventoryGridCells;

        public void SetItemsContainer(ItemsContainer itemsContainer) {

            if (itemsContainer == _itemsContainer)
                return;

            foreach (Transform obj in transform) {
                Destroy(obj.gameObject);
            }

            _itemsContainer = itemsContainer;

            inventoryGridCells = new InventoryGridCell[itemsContainer.maxItemsCount];

            for (var i = 0; i < itemsContainer.maxItemsCount; i++) {

                var cell = Instantiate(inventoryGridCellPrefab.gameObject, transform);

                inventoryGridCells[i] = cell.GetComponent<InventoryGridCell>();
                inventoryGridCells[i].SetItemsContainer(_itemsContainer, i);

            }

        }

    }

}
