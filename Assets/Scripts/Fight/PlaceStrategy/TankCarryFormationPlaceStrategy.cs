using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using UnityEngine;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public class TankCarryFormationPlaceStrategy : TeamPlaceStrategy {

        public override void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells) {

            var tanks = team.aliveChars.Where(a => a.characterData.rangeType == Character.CharacterData.RangeType.Melee);
            var carries = team.aliveChars.Where(a => a.characterData.rangeType == Character.CharacterData.RangeType.Range);

            var lines = new List<CellWithDistance>();

            var firstLineCells = new List<CellWithDistance>(tanks.Count());
            var secondLineCells = new List<CellWithDistance>(carries.Count());

            foreach (var cell in cells) {

                var minCellPair = enemyCells.Select(e => new CellWithDistance(cell, e)).Min();

                firstLineCells.Add(minCellPair);

            }

            var enemyYPositions = enemyCells.Select(e => e.dataPosition.y).Distinct();

            var minYEnemy = enemyYPositions.Min();
            var maxYEnemy = enemyYPositions.Max();

            var linesGroups = cells.GroupBy(c => c.dataPosition.y);

            var orderLines = linesGroups.OrderBy(l => Mathf.Min(Mathf.Abs(l.Key - minYEnemy), Mathf.Abs(l.Key - maxYEnemy)));

            var tanksEnumerator = tanks.GetEnumerator();
            tanksEnumerator.MoveNext();

            var carryEnumerator = carries.GetEnumerator();
            carryEnumerator.MoveNext();

            var usedCells = new List<Cell>();

            foreach (var line in orderLines) {

                if (tanksEnumerator.Current == null)
                    break;

                var lineCells = line.OrderBy(c => c.dataPosition.x).ToArray();

                var middleCell = lineCells[(lineCells.Length / 2)];

                var orderLineCells = lineCells.OrderBy(c => Vector2Int.Distance(c.dataPosition, middleCell.dataPosition));

                foreach (var cellAtLine in orderLineCells) {

                    if (tanksEnumerator.Current == null)
                        break;

                    cellAtLine.StayCtrl(tanksEnumerator.Current);

                    tanksEnumerator.MoveNext();

                    usedCells.Add(cellAtLine);

                }

            }

            foreach (var line in orderLines.Skip(1)) {

                if (carryEnumerator.Current == null)
                    break;

                var lineCells = line.OrderBy(c => c.dataPosition.x).ToArray();

                var middleCell = lineCells[(lineCells.Length / 2)];

                var orderLineCells = lineCells.OrderBy(c => Vector2Int.Distance(c.dataPosition, middleCell.dataPosition));

                foreach (var cellAtLine in orderLineCells) {

                    if (usedCells.Contains(cellAtLine) || carryEnumerator.Current == null)
                        break;

                    cellAtLine.StayCtrl(carryEnumerator.Current);

                    carryEnumerator.MoveNext();

                    usedCells.Add(cellAtLine);

                }

            } 

        }

    }

    public class CellWithDistance : IComparable<CellWithDistance> {

        public readonly Cell firstCell;
        public readonly Cell secondCell;
        public readonly float distance;

        public CellWithDistance(Cell first, Cell second) {

            firstCell = first;
            secondCell = second;

            distance = Vector2Int.Distance(firstCell.dataPosition, secondCell.dataPosition);
        }

        public int CompareTo(CellWithDistance other) => distance.CompareTo(other.distance);

    }
}
