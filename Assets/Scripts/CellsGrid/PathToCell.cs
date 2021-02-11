using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {
    public class PathToCell {

        private CellsGridMng _cellsGridMng;

        public PathToCell(CellsGridMng cellsGridMng) {
            _cellsGridMng = cellsGridMng;
        }

        public Path GetPath(Cell from, Cell to, int range = 1) {

            var paths = new Stack<Path>();

            var usedCells = new List<Cell>() { from };
            var result = new Stack<Cell>();

            var initPath = new Path();
            initPath.cells.Push(from);

            paths.Push(initPath);

            Path pathToClosest = null;
            float closestDistance = float.MaxValue;

            while (paths.Any()) {

                var cellPath = paths.Pop();

                var cell = cellPath.cells.Peek();

                var nextCells = _cellsGridMng.GetNeighbours(cell)
                    .Except(usedCells)
                    .OrderByDescending(n => Vector2Int.Distance(n.dataPosition, to.dataPosition));

                foreach (var nextCell in nextCells) {

                    if ((!nextCell.IsAvailableToStay() && nextCell != to) || usedCells.Contains(nextCell))
                        continue;

                    var p = new Path(cellPath.cells);
                    p.cells.Push(nextCell);

                    if (CellRangeHelper.IsInRange(to.dataPosition, nextCell.dataPosition, range)) {
                        return p;
                    } else {

                        var distance = Vector2Int.Distance(to.dataPosition, nextCell.dataPosition);

                        if (pathToClosest == null || closestDistance > distance) {

                            pathToClosest = p;
                            closestDistance = distance;

                        }

                    }

                    paths.Push(p);
                    usedCells.Add(nextCell);

                }

            }

            if (pathToClosest != null)
                return pathToClosest;

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
            => cells.Reverse().Skip(1).Take(cells.Count - 1);

    }

}
