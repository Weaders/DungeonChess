using Assets.Scripts.UI.DragAndDrop;
using Assets.Scripts.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI {

    public class ItemIcon : MonoBehaviour, IPointerClickHandler {

        public void OnPointerClick(PointerEventData eventData) {

            var item = GetComponent<MoveItem>().currentCell.GetComponent<InventoryGridCell>().GetItem();
            GameMng.current.itemInfoPanel.SetItemData(item);

        }

    }

}
