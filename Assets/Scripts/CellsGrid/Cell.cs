using Assets.Scripts.Character;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.Logging;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CellsGrid {

    public class Cell : MonoBehaviour {

        private CharacterCtrl _characterCtrl;

        public CharacterCtrl characterCtrl {
            get => _characterCtrl;
            private set {

                var oldCtrl = _characterCtrl;
                _characterCtrl = value;
                onStayCtrl.Invoke(oldCtrl, _characterCtrl);

            }
        }

        public Vector2Int dataPosition;

        public CellStateManager stateMng = new CellStateManager();

        public CellActionsContainer cellActionsContainer;

        [SerializeField]
        private CellType cellType = default;

        [SerializeField]
        private Direction _exitDirection;

        [SerializeField]
        private bool _isExit;

        public UnityEvent<CharacterCtrl, CharacterCtrl> onStayCtrl = new UnityEvent<CharacterCtrl, CharacterCtrl>();

        public Direction exitDirection {
            get => _exitDirection;
        }

        public bool isExit {
            get => _isExit;
        }

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private GameObject cellEffect;

        public CharacterCtrl StayCtrlPrefab(CharacterCtrl ctrlPrefab) {

            var ctrlObj = Instantiate(ctrlPrefab);
            var ctrl = ctrlObj.GetComponent<CharacterCtrl>();

            ctrl.Init();

            StayCtrl(ctrl);

            return ctrl;

        }

        private void Start() {
            ChangeColor();
        }

        public void ChangeColor() {

            var colorStore = StaticData.current.colorStore;

            if (GameMng.current.fightMng.isInFight || cellType == CellType.NotUsable || cellType == CellType.ForEnemy || GetState() == CellState.Default) {

                meshRenderer.material.SetColor("_Color", Color.clear);
                meshRenderer.material.SetColor("_OutlineColor", Color.white);
                meshRenderer.material.SetFloat("_WidthOutLine", 0.01f);

                cellEffect.SetActive(false);

            } else {

                // For enemy
                if (cellType == CellType.ForEnemy) {

                    cellEffect.SetActive(false);

                    meshRenderer.material.SetColor("_Color", colorStore.cellEnemy);
                    meshRenderer.material.SetColor("_OutlineColor", colorStore.cellEnemyOutlineCell);

                    // For Player
                } else {

                    if (GetState() == CellState.NotAvailable) {

                        cellEffect.SetActive(false);

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerAllowOutline);

                    } else if (GetState() == CellState.Hover) {

                        cellEffect.SetActive(false);

                        meshRenderer.material.SetColor("_Color", colorStore.cellHover);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellHoverOutlineCell);

                    } else if (GetState() == CellState.Select) {

                        cellEffect.SetActive(true);

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerNotAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerNotAllowOutline);

                    }

                }

            }

        }

        public void StayCtrl(CharacterCtrl ctrl, bool isChangePosition = true) {

            if (ctrl.cell == this)
                return;

            if (ctrl != null) {

                TagLogger<Cell>.Info($"Player stay on cell with position {transform.position}");

                if (ctrl.cell != null) {
                    ctrl.cell.AddState(CellState.Select);
                }

                ctrl.moveCtrl.DisableNavMesh();

                ctrl.transform.SetParent(transform, true);

                if (isChangePosition) {
                    StayCtrlOnlyPosition(ctrl);
                }
                if (ctrl.cell != null) {

                    ctrl.cell.characterCtrl = null;
                    ctrl.cell = this;

                } else {
                    ctrl.cell = this;
                }
                
                

                AddState(CellState.NotAvailable);

                ctrl.moveCtrl.EnableNavMesh();
                characterCtrl = ctrl;

            }

        }

        public void StayCtrlOnlyPosition(CharacterCtrl ctrl) {

            var collider = ctrl.gameObject.GetComponent<BoxCollider>();

            ctrl.transform.localPosition = Vector3.zero;

            var diff = (transform.position.y + 0.01f) - collider.bounds.min.y;

            ctrl.transform.position = new Vector3(ctrl.transform.position.x, ctrl.transform.position.y + diff, ctrl.transform.position.z);
            ctrl.transform.localRotation = Quaternion.identity;

        }

        public void AddState(CellState cellState) {

            stateMng.AddState(cellState);
            ChangeColor();

        }

        public void RemoveState(CellState cellState) {

            stateMng.RemoveState(cellState);
            ChangeColor();

        }

        public void SetCellType(CellType newCellType) {

            cellType = newCellType;
            ChangeColor();

        }

        public bool IsAvailableToStay()
            => (characterCtrl == null || characterCtrl.characterData.stats.isDie) && GetCellType() != CellType.NotUsable;

        public CellState GetState() => stateMng.GetCurrent();

        public bool IsThereState(CellState state) => stateMng.ContainsState(state);

        public CellType GetCellType() => cellType;

        public bool IsExit() => isExit;

        [ContextMenu("Log States")]
        public void LogStates() {

            foreach (var state in stateMng.states) {
                TagLogger<Cell>.Info(state.ToString());
            }

        }

        public enum CellState {
            Default,
            Select,
            NotAvailable,
            Hover
        }

        public enum CellType {
            ForPlayer,
            ForEnemy,
            NotUsable
        }

    }
}
