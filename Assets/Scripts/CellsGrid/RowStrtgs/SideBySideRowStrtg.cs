using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CellsGrid.RowStrtgs {

    public class SideBySideRowStrtg : BaseRowStrtg {

        public enum Orientation {
            Horizontal,
            Vertical
        }

        public int width;

        public bool isFullWidth;

        public Orientation orientation;

        public override IEnumerable<Cell> CalcCellsFor(Cell cell) {

            var widthForSelect = isFullWidth ? GameMng.current.cellsGridMng.widthDataPosition : width;
            var min = GameMng.current.cellsGridMng.minXYDataPosition;
            var max = GameMng.current.cellsGridMng.maxXYDataPosition;
            var positions = new List<Vector2Int>(width * 2);

            for (var x = Mathf.Max(cell.dataPosition.x - widthForSelect, min.x); x <= Mathf.Min(cell.dataPosition.x + widthForSelect, max.x); x++) {

                var position = new Vector2Int(x, cell.dataPosition.y);

                if (position != cell.dataPosition) {
                    positions.Add(position);
                }

            }

            return GameMng.current.cellsGridMng.GetByDataPositions(positions);           

        }
    }
}
