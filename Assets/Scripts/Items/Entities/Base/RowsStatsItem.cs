using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buffs;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.CellsGrid.RowStrtgs;
using Assets.Scripts.Common;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Items.Entities.Base {

    public class RowsStatsItem : StatsItem, IDisplayOnSelect, IBuffSource, IAddCellActions {

        public BaseRowStrtg rowStrtg;

        private Cell[] showedCells;

        private Cell[] cellWithActions;

        [SerializeField]
        private Buff buffPrefab;

        public void AddActionsFor() {

            if (cellWithActions != null)
                RemoveActionsFor();

            cellWithActions = rowStrtg.CalcCellsFor(owner.characterCtrl.GetCellForSelectedDisplay()).ToArray();

            if (cellWithActions != null) {

                foreach (var selectedCell in cellWithActions) {
                    selectedCell.cellActionsContainer.AddBuffAction(buffPrefab, this);
                }

            }

        }

        public void HideFor() {

            if (showedCells != null) {

                foreach (var selectedCell in showedCells) {

                    selectedCell.RemoveState(Cell.CellState.Hover);

                }

                showedCells = null;

            }

        }

        public void RemoveActionsFor() {

            if (cellWithActions != null) {

                foreach (var selectedCell in cellWithActions) {
                    selectedCell.cellActionsContainer.RemoveAction(this);
                }

                cellWithActions = null;

            }

        }

        public void ShowFor() {

            var ctrl = owner.characterCtrl;

            HideFor();

            showedCells = rowStrtg.CalcCellsFor(ctrl.GetCellForSelectedDisplay()).ToArray();

            foreach (var selectedCell in showedCells) {

                selectedCell.AddState(Cell.CellState.Hover);

            }

        }

        protected override void OnDeEquip() {

            base.OnDeEquip();
            HideFor();

        }

    }

}
