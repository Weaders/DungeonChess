using System.Collections.Generic;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.CellsGrid.RowStrtgs;
using Assets.Scripts.Character;
using Assets.Scripts.Common;

namespace Assets.Scripts.Items.Entities.Base {

    public class RowsStatsItem : StatsItem, IDisplayOnSelect {

        public BaseRowStrtg rowStrtg;

        private IEnumerable<Cell> showedCells;

        public void HideFor() {

            if (showedCells != null) {

                foreach (var selectedCell in showedCells) {
                    selectedCell.RemoveState(Cell.CellState.Hover);
                }

                showedCells = null;

            }

        }

        public void ShowFor() {

            var ctrl = owner.characterCtrl;

            HideFor();

            if (ctrl.cell != null) {

                showedCells = rowStrtg.CalcCellsFor(ctrl.GetCellForSelectedDisplay());

                foreach (var selectedCell in showedCells) {
                    selectedCell.AddState(Cell.CellState.Hover);
                }

            }

        }

        protected override void OnDeEquip() {
            base.OnDeEquip();
            HideFor();
        }

    }

}
