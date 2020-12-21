using System;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    public class GameInputCtrl : MonoBehaviour {

        private CharacterCtrl _characterCtrl;

        private Cell _ctrlToSelect;

        private CharacterCtrl selectedCharacterCtrl {
            get => _characterCtrl;
            set {
                _characterCtrl = value;
                onSelectCharacter.Invoke(_characterCtrl);
            }
        }

        private bool isMoveCharacter = false;

        private Vector3 mousePosition;

        [SerializeField]
        private float dragSpeed = .005f;

        public OnSelectCharacter onSelectCharacter = new OnSelectCharacter();

        private void Update() {

            var delta = mousePosition - Input.mousePosition;
            mousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0)) {

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (selectedCharacterCtrl == null) {

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
                                ctrl.moveCtrl.DisableNavMesh();
                                isMoveCharacter = true;

                            } else {
                                selectedCharacterCtrl = ctrl;
                                isMoveCharacter = false;
                            }

                        }

                    }

                } else if (isMoveCharacter) {

                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER))) {

                        var cell = hit.collider.GetComponent<Cell>();

                        if (_ctrlToSelect != cell) {

                            if (_ctrlToSelect != null) {
                                _ctrlToSelect.SetState(Cell.CellState.Select);
                            }

                            _ctrlToSelect = cell;

                            _ctrlToSelect.SetState(Cell.CellState.NotAvailable);

                        }

                    }

                    selectedCharacterCtrl.transform.position -= (new Vector3(delta.x, 0, delta.y) * dragSpeed * Time.deltaTime);

                }

            } else if (Input.GetMouseButtonUp(0) && selectedCharacterCtrl != null) {

                if (isMoveCharacter) {

                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                    if (hit.collider != null) {

                        var cell = hit.collider.GetComponent<Cell>();
                        cell.StayCtrl(selectedCharacterCtrl);

                    }

                }

                selectedCharacterCtrl = null;

            }

        }

        public class OnSelectCharacter : UnityEvent<CharacterCtrl> { }


    }
}
