using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI {

    [RequireComponent(typeof(Collider2D))]
    public class MoveItem : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerClickHandler {

        private MoveItemCell moveItemCell;

        private MoveItemCell cellToPlace = null;

        private RectTransform _rectTransform;

        private Vector3 startPos;

        private RectTransform rectTransform {

            get {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }

        }

        private Dictionary<MoveItemCell, Bounds> cellsWithBounds = new Dictionary<MoveItemCell, Bounds>();

        public void PlaceIn(MoveItemCell itemCell) {

            if (moveItemCell != null) {
                PlaceOut();
            }

            moveItemCell = itemCell;
            itemCell.moveItem = this;
            moveItemCell.state = MoveItemCell.State.Default;

        }

        public void PlaceOut() {

            moveItemCell.state = MoveItemCell.State.Default;
            moveItemCell.moveItem = null;
            moveItemCell = null;

        }

        public MoveItemCell currentCell => moveItemCell;

        public void OnDrag(PointerEventData eventData) {

            transform.position = transform.position + (Vector3)eventData.delta;

            var minDistance = float.MaxValue;
            MoveItemCell cellToSel = null;

            foreach (var kv in cellsWithBounds) {

                var distance = Vector2.Distance(kv.Value.center, rectTransform.rect.center);

                if (distance < minDistance) {

                    distance = minDistance;
                    cellToSel = kv.Key;

                }

            }

            if (cellToPlace != null && cellToPlace != cellToSel) {
                cellToPlace.state = MoveItemCell.State.Default;
            }

            cellToPlace = cellToSel;

            if (cellToPlace != null) {

                if (cellToPlace.moveItem == null) {
                    cellToPlace.state = MoveItemCell.State.AvaliableForSelect;
                } else {
                    cellToPlace.state = MoveItemCell.State.NotAvailableForSelect;
                }

            }


        }

        public void OnEndDrag(PointerEventData eventData) {

            if (cellToPlace != null && cellToPlace.state == MoveItemCell.State.AvaliableForSelect)
                PlaceIn(cellToPlace);
            else {

                if (cellToPlace != null) {
                    cellToPlace.state = MoveItemCell.State.Default;
                }

                transform.localPosition = startPos;
            }
                

        }

        private void OnTriggerEnter2D(Collider2D collision) {

            if (collision.CompareTag(TagsStore.MOVE_ITEM_CELL)) {

                var cell = collision.GetComponent<MoveItemCell>();

                if (cell != null) {
                    cellsWithBounds.Add(cell, collision.bounds);
                }

            }

        }

        private void OnTriggerExit2D(Collider2D collision) {

            if (collision.CompareTag(TagsStore.MOVE_ITEM_CELL)) {

                var cell = collision.GetComponent<MoveItemCell>();

                if (cell != null) {
                    cellsWithBounds.Remove(cell);
                }

            }

        }

        public void OnBeginDrag(PointerEventData eventData) {
            cellToPlace = null;
            startPos = transform.localPosition;
            cellsWithBounds.Clear();
        }

        public void OnPointerClick(PointerEventData eventData) {

            var moveItem = GameMng.current.moveItemFactory.Get(this);
            moveItem.ClickHandle(this);

        }
    }

}
