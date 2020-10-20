using Assets.Scripts.Character;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CellsGrid {
    public class Cell : MonoBehaviour {

        public UnityEvent onClick = new UnityEvent();

        public Vector2Int dataPosition;

        private CellState state = default;

        [SerializeField]
        private CellType cellType = default;

        [SerializeField]
        private Direction _exitDirection;

        [SerializeField]
        private bool _isExit;

        public Direction exitDirection { 
            get => _exitDirection;
        }

        public bool isExit {
            get => _isExit;
        }

        [SerializeField]
        private MeshRenderer meshRenderer;

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

            if (GameMng.current.fightMng.isInFight) {

                meshRenderer.material.SetColor("_Color", Color.white);
                meshRenderer.material.SetColor("_OutlineColor", Color.white);

            } else {

                if (cellType == CellType.ForEnemy) {

                    meshRenderer.material.SetColor("_Color", colorStore.cellEnemy);
                    meshRenderer.material.SetColor("_OutlineColor", colorStore.cellEnemyOutlineCell);

                } else {

                    if (state == CellState.NotAvailable) {

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerAllowOutline);

                    } else {

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerNotAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerNotAllowOutline);

                    }

                }

            }

            

        }

        public void StayCtrl(CharacterCtrl ctrl) {

            ctrl.moveCtrl.DisableNavMesh();

            ctrl.transform.SetParent(transform, true);
            ctrl.transform.localPosition = Vector3.zero;

            ctrl.cell = this;

            SetState(CellState.NotAvailable);

            ctrl.moveCtrl.EnableNavMesh();

        }

        public void SetState(CellState cellState) {

            state = cellState;
            ChangeColor();

        }

        public void SetCellType(CellType newCellType) {

            cellType = newCellType;
            ChangeColor();

        }

        public CellState GetState() => state;

        public CellType GetCellType() => cellType;

        public bool IsExit() => isExit;

        public enum CellState {
            Select,
            NotAvailable
        }

        public enum CellType {
            ForPlayer,
            ForEnemy
        }

    }
}
