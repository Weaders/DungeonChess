﻿using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Items;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.DragAndDrop {

    [RequireComponent(typeof(Collider2D))]
    public class MoveItem : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerClickHandler {

        private MoveItemCell moveItemCell;

        private MoveItemCell cellToPlace = null;

        private RectTransform _rectTransform;

        private Vector3 startPos;

        private Transform startParent;

        public UnityEvent onDestoy = new UnityEvent();

        [SerializeField]
        private Text countItemsText;

        private GameObject canvas = null;

        /// <summary>
        /// Character ctrl, for that will be equiped item, by over mouse
        /// </summary>
        private CharacterCtrl ctrlToEquip;

        private RectTransform rectTransform {

            get {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();

                return _rectTransform;
            }

        }

        private Dictionary<MoveItemCell, Bounds> cellsWithBounds = new Dictionary<MoveItemCell, Bounds>();

        public MoveItemCell currentCell {
            get => moveItemCell;
            set => moveItemCell = value;
        }

        private void Start() {

            var item = GameMng.current.moveItemFactory.Get(this);

            item.onDestroy.AddListener(() => {
                Destroy(gameObject);
            });

            if (item.count != null) {
                countItemsText.Subscribe(item.count);
            }

        }

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

            if (cellToSel == null) {

                if (cellToPlace != null) {
                    
                    cellToPlace.state = MoveItemCell.State.Default;
                    cellToPlace = moveItemCell;

                }

                ctrlToEquip = GameMng.current.gameInputCtrl.overCharacterCtrl;
                
                return;

            }

            ctrlToEquip = null;

            if (cellToSel.state == MoveItemCell.State.Locked || cellToSel == moveItemCell)
                return;

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

            var oldCell = currentCell;

            GameMng.current.gameInputCtrl.blockUI = false;

            if (ctrlToEquip != null) {

                var itemData = GameMng.current.moveItemFactory.Get<ItemData>(this);
                ctrlToEquip.characterData.itemsContainer.Add(itemData);

                if (IsNeedMoveBack()) {
                    GameMng.current.moveItemSystem.PlaceItem(this, oldCell);
                }

                return;

            } else {

                if (cellToPlace != null && cellToPlace.state == MoveItemCell.State.AvaliableForSelect) {
                    
                    GameMng.current.moveItemSystem.PlaceItem(this, cellToPlace);

                    if (IsNeedMoveBack()) {
                        GameMng.current.moveItemSystem.PlaceItem(this, oldCell);
                    }

                } else {

                    if (cellToPlace != null) {
                        cellToPlace.state = MoveItemCell.State.Default;
                    }

                    transform.SetParent(startParent);
                    transform.localPosition = startPos;

                }

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

            GameMng.current.gameInputCtrl.blockUI = true;
            cellToPlace = null;
            startPos = transform.localPosition;
            startParent = transform.parent;
            cellsWithBounds.Clear();

            if (canvas == null)
                canvas = GameObject.FindGameObjectWithTag(TagsStore.MAIN_CANVAS);

            transform.SetParent(canvas.transform, true);
            transform.SetAsLastSibling();

        }

        public void OnPointerClick(PointerEventData eventData) {

            var moveItem = GameMng.current.moveItemFactory.Get(this);
            moveItem.ClickHandle(this);

        }

        public bool IsNeedMoveBack() {

            if (!isActiveAndEnabled)
                return false;

            var moveItem = GameMng.current.moveItemFactory.Get(this);
            return moveItem.IsNeedMoveBack();

        }

        private void OnDestroy() {
            onDestoy.Invoke();
        }
    }

}
