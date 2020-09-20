using Assets.Scripts.Character;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CellsGrid {
    public class Cell : MonoBehaviour {

        public UnityEvent onClick = new UnityEvent();

        public Vector2Int dataPosition;

        [SerializeField]
        private CellState state = default;

        [SerializeField]
        private CellType cellType = default;

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private DungeonRoomCells exitFromRoom;

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

        private void ChangeColor() {

            var colorStore = StaticData.current.colorStore;

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

        public void SetExit(DungeonRoomCells exitVal) {
            exitFromRoom = exitVal;
        }

        public bool IsExit() => exitFromRoom != null;

        public DungeonRoomCells GetExit() => exitFromRoom;

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
