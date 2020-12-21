using System.Collections;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.Logging;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CellsGrid {

    public class Cell : MonoBehaviour {

        public UnityEvent onClick = new UnityEvent();

        public CharacterCtrl characterCtrl { get; private set; }

        //[ReadOnly]
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

            if (GameMng.current.fightMng.isInFight || cellType == CellType.NotUsable || cellType == CellType.ForEnemy) {

                meshRenderer.material.SetColor("_Color", Color.clear);
                meshRenderer.material.SetColor("_OutlineColor", Color.white);
                meshRenderer.material.SetFloat("_WidthOutLine", 0.01f);

                cellEffect.SetActive(false);

            } else {

                if (cellType == CellType.ForEnemy) {

                    cellEffect.SetActive(false);

                    meshRenderer.material.SetColor("_Color", colorStore.cellEnemy);
                    meshRenderer.material.SetColor("_OutlineColor", colorStore.cellEnemyOutlineCell);

                } else {

                    if (state == CellState.NotAvailable) {

                        cellEffect.SetActive(false);

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerAllowOutline);

                    } else {

                        cellEffect.SetActive(true);

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerNotAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerNotAllowOutline);

                    }

                }

            }

            

        }

        public void StayCtrl(CharacterCtrl ctrl, bool isChangePosition = true) {

            if (ctrl != null) {

                if (ctrl.cell != null) {

                    ctrl.cell.characterCtrl = null;
                    ctrl.cell = null;

                }

                TagLogger<Cell>.Info($"Player stay on cell with position {transform.position}");

                if (ctrl.cell != null) {
                    ctrl.cell.SetState(CellState.Select);
                }


                ctrl.moveCtrl.DisableNavMesh();

                ctrl.transform.SetParent(transform, true);

                if (isChangePosition) {
                    ctrl.transform.localPosition = Vector3.zero;
                    ctrl.transform.localRotation = Quaternion.identity;
                }
                

                ctrl.cell = this;

                SetState(CellState.NotAvailable);

                ctrl.moveCtrl.EnableNavMesh();
                characterCtrl = ctrl;

            }

        }

        public void SetState(CellState cellState) {

            state = cellState;
            ChangeColor();

        }

        public void SetCellType(CellType newCellType) {

            cellType = newCellType;
            ChangeColor();

        }

        public bool IsAvailableToStay()
            => characterCtrl == null || characterCtrl.characterData.stats.isDie;

        public CellState GetState() => state;

        public CellType GetCellType() => cellType;

        public bool IsExit() => isExit;

        public enum CellState {
            Select,
            NotAvailable
        }

        public enum CellType {
            ForPlayer,
            ForEnemy,
            NotUsable
        }

    }
}
