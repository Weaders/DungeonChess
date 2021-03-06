﻿using Assets.Scripts.Character;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.Effects;
using Assets.Scripts.Logging;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CellsGrid {

    public enum EnemyPrefer {
        WithJump
    }

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

        public EnemyPrefer[] enemyPrefer;

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

        public bool isMovedToThisCell {
            get => movedToThisCellCtrl != null && !movedToThisCellCtrl.characterData.stats.isDie;
        }

        public CharacterCtrl movedToThisCellCtrl { get; private set; }

        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private GameObject cellEffect;

        [SerializeField]
        private Effect stayEffect;

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

        public void SetMaterial(Material material) {

            meshRenderer.material = material;
            ChangeColor();

        }

        public void ChangeColor() {

            if (!gameObject.activeInHierarchy)
                return;

            var colorStore = StaticData.current.colorStore;

            if (GameMng.current.fightMng.isInFight || cellType == CellType.NotUsable || cellType == CellType.ForEnemy || GetState() == CellState.Default) {

                meshRenderer.material.SetColor("_Color", Color.clear);
                meshRenderer.material.SetColor("_OutlineColor", Color.white);
                meshRenderer.material.SetFloat("_WidthOutLine", 0.01f);

                ChangeCellEffectActive(false);


            } else {

                // For enemy
                if (cellType == CellType.ForEnemy) {

                    cellEffect.SetActive(false);

                    meshRenderer.material.SetColor("_Color", colorStore.cellEnemy);
                    meshRenderer.material.SetColor("_OutlineColor", colorStore.cellEnemyOutlineCell);

                    // For Player
                } else {

                    if (GetState() == CellState.NotAvailable) {

                        ChangeCellEffectActive(false);

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerAllowOutline);

                    } else if (GetState() == CellState.Hover) {

                        ChangeCellEffectActive(false);

                        meshRenderer.material.SetColor("_Color", colorStore.cellHover);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellHoverOutlineCell);

                    } else if (GetState() == CellState.Select) {

                        ChangeCellEffectActive(true);

                        meshRenderer.material.SetColor("_Color", colorStore.cellPlayerNotAllow);
                        meshRenderer.material.SetColor("_OutlineColor", colorStore.cellPlayerNotAllowOutline);

                    }

                }

            }

            

        }

        public void ChangeCellEffectActive(bool active) {

#if UNITY_ANDROID
            cellEffect.SetActive(false);
#else
            cellEffect.SetActive(active);
#endif

        }

        public void MovedToThisCell(CharacterCtrl characterCtrl) {
            movedToThisCellCtrl = characterCtrl;
        }

        public void StayCtrl(CharacterCtrl ctrl, bool isChangePosition = true, bool changeState = true, bool runStayEffect = false) {

            if ((ctrl == null && characterCtrl == null) || (ctrl != null && ctrl.characterData.cell == this))
                return;

            if (runStayEffect && stayEffect != null)
                stayEffect.Play();

            if (ctrl != null) {

                TagLogger<Cell>.Info($"Player stay on cell with position {transform.position}");

                if (ctrl.characterData.cell != null && changeState) {

                    ctrl.characterData.cell.AddState(CellState.Select);
                    ctrl.characterData.cell.RemoveState(CellState.NotAvailable);

                }

                ctrl.moveCtrl.DisableNavMesh();

                ctrl.transform.SetParent(transform, true);

                if (isChangePosition) {
                    StayCtrlOnlyPosition(ctrl);
                }

                if (ctrl.characterData.cell != null) {

                    ctrl.characterData.cell.characterCtrl = null;
                    ctrl.characterData.cell = this;

                } else {
                    ctrl.characterData.cell = this;
                }

                if (changeState)
                    AddState(CellState.NotAvailable);

                ctrl.moveCtrl.EnableNavMesh();
                characterCtrl = ctrl;

            } else {

                characterCtrl.characterData.cell = null;
                characterCtrl = null;

                if (changeState)
                    RemoveState(CellState.NotAvailable);

            }

            movedToThisCellCtrl = null;

        }

        public void StayCtrlOnlyPosition(CharacterCtrl ctrl) {

            var collider = ctrl.gameObject.GetComponent<BoxCollider>();

            if (collider != null) {

                ctrl.transform.localPosition = Vector3.zero;

                var diff = (transform.position.y + 0.01f) - collider.bounds.min.y;

                ctrl.transform.position = new Vector3(ctrl.transform.position.x, ctrl.transform.position.y + diff, ctrl.transform.position.z);
                ctrl.transform.localRotation = Quaternion.identity;

            }

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
            => (characterCtrl == null || characterCtrl.characterData.stats.isDie) && GetCellType() != CellType.NotUsable && !isMovedToThisCell;

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
