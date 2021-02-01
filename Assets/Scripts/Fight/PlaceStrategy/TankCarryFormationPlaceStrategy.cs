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

            var firstLineCells = new List<CellWithDistance>(tanks.Count());
            var secondLineCells = new List<CellWithDistance>(carries.Count());

            foreach (var cell in cells) {

                var usedFirstLineCell = firstLineCells.Select(flc => flc.firstCell);

                var minCellPair = enemyCells.Select(e => new CellWithDistance(cell, e)).Min();

                firstLineCells.Add(minCellPair);

            }

            firstLineCells.Sort((a, b) => {

                var compareFirstReturn = a.CompareTo(b);

                if (compareFirstReturn == 0)
                    return (a.firstCell.dataPosition.x + a.firstCell.dataPosition.y).CompareTo(b.firstCell.dataPosition.x + b.firstCell.dataPosition.y);

                return compareFirstReturn;

            });

            var minDistance = firstLineCells[0].distance;

            firstLineCells = firstLineCells.Where(flc => flc.distance == minDistance).ToList();

            if (firstLineCells.Count == cells.Count())
                firstLineCells = firstLineCells.Take(tanks.Count()).ToList();

            var exceptFirstLine = cells.Except(firstLineCells.Select(flc => flc.firstCell)).ToList();

            foreach (var fc in firstLineCells) {

                exceptFirstLine = exceptFirstLine.Except(secondLineCells.Select(s => s.firstCell)).ToList();

                var nearCell = exceptFirstLine.Select(e => new CellWithDistance(e, fc.firstCell)).Min();

                if (nearCell != null)
                    secondLineCells.Add(nearCell);

            }

            var tanksEnumerator = tanks.GetEnumerator();
            tanksEnumerator.MoveNext();

            for (var i = firstLineCells.Count / 2 - tanks.Count() / 2; i < firstLineCells.Count; i++) {

                if (tanksEnumerator.Current == null)
                    break;

                firstLineCells[i].firstCell.StayCtrl(tanksEnumerator.Current);

                tanksEnumerator.MoveNext();

            }


            var carryEnumerator = carries.GetEnumerator();
            carryEnumerator.MoveNext();

            for (var i = secondLineCells.Count / 2 - carries.Count() / 2; i < secondLineCells.Count; i++) {

                if (carryEnumerator.Current == null)
                    break;

                secondLineCells[i].firstCell.StayCtrl(carryEnumerator.Current);

                carryEnumerator.MoveNext();

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
