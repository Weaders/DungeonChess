using Assets.Scripts.Common.Exceptions;
using UnityEngine;

namespace Assets.Scripts.UI.DragAndDrop {

    public class MoveItemSystem {

        public void PlaceItem(MoveItem moveItem, MoveItemCell cell, bool fireEvents = true) {

            if (cell == null)
                return;

            if (moveItem == cell.moveItem)
                return;

            var oldCell = moveItem.currentCell;

            if (moveItem != null && moveItem.currentCell != null && moveItem.currentCell != cell) {
                moveItem.currentCell.SetMoveItem(null);
            }

            if (cell.moveItem != null && moveItem != null) {
                throw new GameException("Cell already exists move item");
            }

            if (moveItem != null) {

                moveItem.transform.SetParent(cell.transform);
                moveItem.transform.localPosition = Vector3.zero;
                moveItem.transform.localScale = Vector3.one;
                cell.state = MoveItemCell.State.Default;

                moveItem.currentCell = cell;

            }

            cell.SetMoveItem(moveItem, fireEvents);

            // Used for one time used item
            if (moveItem.IsNeedMoveBack() && moveItem.isActiveAndEnabled)
                PlaceItem(moveItem, oldCell, fireEvents);

        }

    }

}
