using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public class MaxDistancePlaceStrategy : TeamPlaceStrategy {

        public override void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells) {

            var cellsForSelect = new Cell[team.aliveChars.Count()];

            for (var i = 0; i < cellsForSelect.Length; i++) {

                if (i == 0) {
                    cellsForSelect[0] = cells.MinElement(c => c.dataPosition.x + c.dataPosition.y);
                } else {
                    
                    cellsForSelect[i] = cells
                        .Where(c => !cellsForSelect.Contains(c))
                        .MaxElement(c => 
                            cellsForSelect.Where(xc => xc != null).Sum(x => Vector2Int.Distance(x.dataPosition, c.dataPosition)
                        )
                    );

                }

            }

            var cellI = 0;

            foreach (var character in team.aliveChars) {

                TagLogger<MaxDistancePlaceStrategy>.Info($"{character.name} spawned for {cellsForSelect[cellI].dataPosition}");

                cellsForSelect[cellI].StayCtrl(character);
                cellI++;

            }

        }

    }
}
