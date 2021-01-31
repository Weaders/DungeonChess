using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Character {

    public class GameInputCtrl : MonoBehaviour {

        private CharacterCtrl _characterCtrl;

        private Cell _cellToSelect;

        public CharacterCtrl selectedCharacterCtrl {
            get => _characterCtrl;
            private set {

                var oldVal = _characterCtrl;
                _characterCtrl = value;

                onChangeSelectCharacter.Invoke(oldVal, _characterCtrl);

                if (oldVal != null)
                    oldVal.isSelected = false;

                if (value != null)
                    value.isSelected = true;

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

        private Vector3 _offsetDraggedCtrl;

        private Vector3 lastDragHit;

        private Vector3 lastSourceRay;

        private Vector3 sourceDir;

        private void Update() {

            var delta = mousePosition - Input.mousePosition;
            mousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0)) {

                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (_draggedCtrl != null) {

                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER))) {

                        var cell = hit.collider.GetComponent<Cell>();

                        if (_cellToSelect != cell && cell != null && (cell.characterCtrl == _draggedCtrl || cell.IsAvailableToStay()) && cell.GetCellType() == Cell.CellType.ForPlayer) {

                            if (_cellToSelect != null) {
                                _cellToSelect.RemoveState(Cell.CellState.NotAvailable);
                            }

                            _cellToSelect = cell;
                            _draggedCtrl.OnDraggableToCell(_cellToSelect);

                            _cellToSelect.AddState(Cell.CellState.NotAvailable);

                        }

                        //var offset = lastDragHit - hit.point;

                        var offset2 = _draggedCtrl.cell.transform.position - (hit.point - _offsetDraggedCtrl);

                        var from = Camera.main.transform.position - offset2;

                        var ray2 = new Ray(from, sourceDir);

                        //Debug.DrawRay(from, sourceDir * 100f, Color.red, 1000f);
                        //Debug.DrawLine(from, from + Vector3.one * .1f, Color.green, 100f);

                        Physics.Raycast(ray2, out RaycastHit hit2, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                        //var r = new Ray(lastSourceRay + offset, ray.direction);

                        //Physics.Raycast(r, out RaycastHit hit2, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 100);


                        //selectedCharacterCtrl.transform.position -= (new Vector3(delta.x, 0, delta.y) * dragSpeed * Time.deltaTime);

                        //var newPos = new Vector3(hit.point.x, selectedCharacterCtrl.transform.position.y, hit.point.z);
                        //var offset = new Vector3(_offsetDraggedCtrl.x, 0, _offsetDraggedCtrl.z);
                        //var targetPos = newPos + offset * 2;
                        //var offset = hit.point - lastDragHit;
                        //lastDragHit = hit.point;

                        ////Debug.Log($"{newPos}|{offset}|{targetPos}");
                        //_draggedCtrl.transform.position = new Vector3(
                        //    _draggedCtrl.transform.position.x - offset.x * .9f,
                        //    _draggedCtrl.transform.position.y,
                        //    _draggedCtrl.transform.position.z - offset.z * .9f
                        //);

                        // Why .87? This is hard calculaton! And this is very secret!
                        var offset = (lastDragHit - hit2.point) * .87f;

                        //_draggedCtrl.transform.position = hit2.point - _offsetDraggedCtrl;

                        _draggedCtrl.transform.position = new Vector3(
                            _draggedCtrl.transform.position.x - offset.x,
                            _draggedCtrl.transform.position.y,
                            _draggedCtrl.transform.position.z - offset.z
                        );

                        lastDragHit = hit2.point;

                    }

                } else {


                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CHARACTER_LAYER, LayersStore.CELL_LAYER))) {

                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer(LayersStore.CELL_LAYER)) {

                            if (GameMng.current.buyPanelUI.selectedBuyData != null && GameMng.current.buyPanelUI.selectedBuyData.IsCanBuy()) {

                                var cell = hit.collider.GetComponent<Cell>();

                                if (cell != null && cell.IsAvailableToStay() && cell.IsThereState(Cell.CellState.Select) && cell.GetCellType() == Cell.CellType.ForPlayer) {

                                    var ctrl = GameMng.current.buyMng.Buy(GameMng.current.buyPanelUI.selectedBuyData);
                                    cell.StayCtrl(ctrl);

                                }

                            }

                        } else {

                            var ctrl = hit.collider.GetComponent<CharacterCtrl>();

                            if (GameMng.current.fightMng.GetTeamSide(ctrl) == Fight.TeamSide.Player) {

                                selectedCharacterCtrl = ctrl;

                                _cellToSelect = selectedCharacterCtrl.cell;

                                ctrl.moveCtrl.DisableNavMesh();
                                _draggedCtrl = ctrl;

                                Physics.Raycast(ray, out RaycastHit hitCell, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                                lastDragHit = hitCell.point;
                                lastSourceRay = Camera.main.transform.position;
                                _offsetDraggedCtrl = hitCell.point - _draggedCtrl.transform.position ;
                                sourceDir = ray.direction;

                            } else {
                                selectedCharacterCtrl = ctrl;
                                _draggedCtrl = null;
                            }

                        }

                    }

                }

            } else if (Input.GetMouseButtonUp(0)) {

                if (_draggedCtrl != null) {

                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                    if (hit.collider != null) {

                        var cell = hit.collider.GetComponent<Cell>();

                        if (cell.GetCellType() == Cell.CellType.ForPlayer && cell.IsAvailableToStay() && cell.IsThereState(Cell.CellState.Select)) {
                            cell.StayCtrl(selectedCharacterCtrl);
                        } else {
                            var cellToMove = selectedCharacterCtrl.GetCellForSelectedDisplay();

                            if (cellToMove != selectedCharacterCtrl.cell)
                                cellToMove.StayCtrl(selectedCharacterCtrl);
                        }

                    }

                    selectedCharacterCtrl.OnDraggableToCell(null);
                    _draggedCtrl = null;
                    selectedCharacterCtrl.cell.StayCtrlOnlyPosition(selectedCharacterCtrl);

                }

            }

        }

        public class OnChangedSelectedCharacter : UnityEvent<CharacterCtrl, CharacterCtrl> { }

    }
}
