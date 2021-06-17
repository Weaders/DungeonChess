using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Character {

    public class GameInputCtrl : MonoBehaviour {

        private CharacterCtrl _characterCtrl;

        private Cell _cellToSelect;

        public CharacterCtrl overCharacterCtrl {
            get;
            private set;
        }

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

        /// <summary>
        /// First val old val, second new val
        /// </summary>
        public OnChangedSelectedCharacter onChangeSelectCharacter = new OnChangedSelectedCharacter();

        private CharacterCtrl _draggedCtrl;

        public CharacterCtrl draggedCtrl => _draggedCtrl;

        private Vector3 _offsetDraggedCtrl;

        private Vector3 lastDragHit;

        private Vector3 sourceDir;

        public void Init() {

            GameMng.current.roomCtrl.onMoveToNextRoom.AddListener(() => {

                if (selectedCharacterCtrl != null && selectedCharacterCtrl.characterData.stats.isDie)
                    selectedCharacterCtrl = null;

            });

        }

        private void Update() {


            if (IsUIBlocked())
                return;


            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 500f, LayerMask.GetMask(LayersStore.CHARACTER_LAYER))) {
                overCharacterCtrl = raycastHit.collider.GetComponent<CharacterCtrl>();
            }

            if (Input.GetMouseButton(0)) {

                if (_draggedCtrl != null) {

                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER))) {

                        var cell = hit.collider.GetComponent<Cell>();

                        if (_cellToSelect != cell && cell != null && (cell.characterCtrl == _draggedCtrl || cell.IsAvailableToStay()) && cell.GetCellType() == Cell.CellType.ForPlayer) {

                            _cellToSelect = cell;
                            _draggedCtrl.OnDraggableToCell(_cellToSelect);

                        }

                        var offset2 = _draggedCtrl.characterData.cell.transform.position - (hit.point - _offsetDraggedCtrl);

                        var from = Camera.main.transform.position - offset2;

                        var ray2 = new Ray(from, sourceDir);

                        Physics.Raycast(ray2, out RaycastHit hit2, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                        // Why .87? This is hard calculaton! And this is secret!
                        var offset = (lastDragHit - hit2.point) * .87f;

                        _draggedCtrl.transform.position = new Vector3(
                            _draggedCtrl.transform.position.x - offset.x,
                            _draggedCtrl.transform.position.y,
                            _draggedCtrl.transform.position.z - offset.z
                        );

                        lastDragHit = hit2.point;

                    }

                } else {

                    if (overCharacterCtrl != null) {

                        var ctrl = overCharacterCtrl;

                        if (ctrl != null && GameMng.current.fightMng.GetTeamSide(ctrl) == Fight.TeamSide.Player && !GameMng.current.fightMng.isInFight) {

                            selectedCharacterCtrl = ctrl;

                            _cellToSelect = selectedCharacterCtrl.characterData.cell;

                            ctrl.moveCtrl.DisableNavMesh();
                            _draggedCtrl = ctrl;

                            Physics.Raycast(ray, out RaycastHit hitCell, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER));

                            lastDragHit = hitCell.point;
                            _offsetDraggedCtrl = hitCell.point - _draggedCtrl.transform.position;
                            sourceDir = ray.direction;

                        } else {
                            selectedCharacterCtrl = ctrl;
                            _draggedCtrl = null;
                        }

                    }

                }

            } else if (Input.GetMouseButtonUp(0)) {

                if (_draggedCtrl != null) {

                    selectedCharacterCtrl.OnDraggableToCell(null);
                    _draggedCtrl = null;

                    if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER)) && hit.collider != null) {

                        var cell = hit.collider.GetComponent<Cell>();

                        if (cell.GetCellType() == Cell.CellType.ForPlayer && cell.IsAvailableToStay() && cell.IsThereState(Cell.CellState.Select)) {
                            cell.StayCtrl(selectedCharacterCtrl);
                        } else {

                            var cellToMove = selectedCharacterCtrl.GetCellForSelectedDisplay();

                            if (cellToMove != selectedCharacterCtrl.characterData.cell)
                                cellToMove.StayCtrl(selectedCharacterCtrl);
                        }

                    }

                    selectedCharacterCtrl.characterData.cell.StayCtrlOnlyPosition(selectedCharacterCtrl);

                } else {

                    if (IsUIBlocked())
                        return;

                    if (GameMng.current.buyPanelUI.selectedBuyData != null && GameMng.current.buyPanelUI.selectedBuyData.IsCanBuy()) {

                        if (Physics.Raycast(ray, out RaycastHit hit, 500f, LayerMask.GetMask(LayersStore.CELL_LAYER))) {

                            var cell = hit.collider.GetComponent<Cell>();

                            if (cell != null && cell.IsAvailableToStay() && cell.IsThereState(Cell.CellState.Select) && cell.GetCellType() == Cell.CellType.ForPlayer) {

                                var ctrl = GameMng.current.buyMng.Buy(GameMng.current.buyPanelUI.selectedBuyData);
                                cell.StayCtrl(ctrl);

                            }

                        }

                    }

                }

            }

        }

        private bool IsUIBlocked() {

            return GameMng.current.dragByMouse.isDragged ||
                GameMng.current.messagePanel.IsShowed ||
                    GameMng.current.selectPanel.IsShowed ||
                        (EventSystem.current.IsPointerOverGameObject()
                        && EventSystem.current.currentSelectedGameObject != null
                        && EventSystem.current.currentSelectedGameObject.tag != TagsStore.DRAG_BY_MOUSE);
        }

        public class OnChangedSelectedCharacter : UnityEvent<CharacterCtrl, CharacterCtrl> { }

    }
}
