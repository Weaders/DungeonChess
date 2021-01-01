using System;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    public class GameInputCtrl : MonoBehaviour {

        private CharacterCtrl _characterCtrl;

        private Cell _cellToSelect;

        private CharacterCtrl selectedCharacterCtrl {
            get => _characterCtrl;
            set {

                var oldVal = _characterCtrl;                
                _characterCtrl = value;

                onChangeSelectCharacter.Invoke(oldVal, _characterCtrl);
            }
        }

        private Vector3 mousePosition;

        [SerializeField]
        private float dragSpeed = .005f;

        /// <summary>
        /// First val old val, second new val
        /// </summary>
        public OnChangedSelectedCharacter onChangeSelectCharacter = new OnChangedSelectedCharacter();

        private CharacterCtrl _draggedCtrl;

        private void Update() {

            var delta = mousePosition - Input.mousePosition;
            mousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0)) {

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (_draggedCtrl != null) {

                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER))) {

                        var cell = hit.collider.GetComponent<Cell>();

                        if (_cellToSelect != cell && cell != null && cell.IsAvailableToStay()) {

                            if (_cellToSelect != null) {
                                _cellToSelect.RemoveState(Cell.CellState.NotAvailable);
                            }

                            _cellToSelect = cell;
                            _draggedCtrl.OnDraggableToCell(_cellToSelect);

                            _cellToSelect.AddState(Cell.CellState.NotAvailable);

                        }

                    }

                    selectedCharacterCtrl.transform.position -= (new Vector3(delta.x, 0, delta.y) * dragSpeed * Time.deltaTime);

                } else {

                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CHARACTER_LAYER, LayersStore.CELL_LAYER))) {

                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(LayersStore.CELL_LAYER)) {

                            if (GameMng.current.buyPanelUI.selectedBuyData != null && GameMng.current.buyPanelUI.selectedBuyData.IsCanBuy()) {

                                var cell = hit.collider.GetComponent<Cell>();

                                if (cell != null && cell.GetState() == Cell.CellState.Select && cell.GetCellType() == Cell.CellType.ForPlayer) {

                                    var ctrl = GameMng.current.buyMng.Buy(GameMng.current.buyPanelUI.selectedBuyData);
                                    cell.StayCtrl(ctrl);

                                    if (!GameMng.current.buyPanelUI.selectedBuyData.IsCanBuy()) {
                                        GameMng.current.buyPanelUI.selectedBuyData = null;
                                    }

                                }

                            }

                        } else {

                            var ctrl = hit.collider.GetComponent<CharacterCtrl>();

                            if (GameMng.current.fightMng.GetTeamSide(ctrl) == Fight.TeamSide.Player) {

                                selectedCharacterCtrl = ctrl;
                                
                                _cellToSelect = selectedCharacterCtrl.cell;

                                ctrl.moveCtrl.DisableNavMesh();
                                _draggedCtrl = ctrl;

                            } else {
                                selectedCharacterCtrl = ctrl;
                                _draggedCtrl = null;
                            }

                        }

                    }

                }

            } else if (Input.GetMouseButtonUp(0) ) {

                if (_draggedCtrl != null) {

                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                    if (hit.collider != null) {

                        var cell = hit.collider.GetComponent<Cell>();
                        cell.StayCtrl(selectedCharacterCtrl);
                        

                    }

                    selectedCharacterCtrl.OnDraggableToCell(null);
                    _draggedCtrl = null;

                }

            }

        }

        public class OnChangedSelectedCharacter : UnityEvent<CharacterCtrl, CharacterCtrl> { }


    }
}
