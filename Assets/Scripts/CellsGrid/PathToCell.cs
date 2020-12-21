using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {
    public class PathToCell {

        private CellsGridMng _cellsGridMng;

        public PathToCell(CellsGridMng cellsGridMng) {
            _cellsGridMng = cellsGridMng;
        }

        public Path GetPath(Cell from, Cell to) {

            var paths = new Stack<Path>();

            var usedCells = new List<Cell>() { from };

            var initPath = new Path();
            initPath.cells.Push(from);

            paths.Push(initPath);

            while (paths.Any()) {

                var cellPath = paths.Pop();

                var cell = cellPath.cells.Peek();

                var neighbours = _cellsGridMng.GetNeighbours(cell);

                var nextCells = neighbours
                    .Except(usedCells)
                    .OrderByDescending(n => Vector2Int.Distance(n.dataPosition, to.dataPosition));

                foreach (var nextCell in nextCells) {

                    if (!nextCell.IsAvailableToStay() && nextCell != to)
                        continue;

                    var p = new Path(cellPath.cells);
                    p.cells.Push(nextCell);

                    if (nextCell == to)
                        return p;

                    paths.Push(p);
                    usedCells.Add(nextCell);

                }

            }

            return null;

        }

    }

    public class Path {

        public Path() { }

        public Path(IEnumerable<Cell> _cells) {
            cells = new Stack<Cell>(_cells.Reverse());
        }

        public Stack<Cell> cells = new Stack<Cell>();

        public IEnumerable<Cell> GetToMovePath()
            => cells.Reverse().Skip(1).Take(cells.Count - 2);

    }

}
