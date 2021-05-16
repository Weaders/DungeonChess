using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public class MiddleFormation : TeamPlaceStrategy {

        public override void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells)
            => Place(team.aliveChars, cells, enemyCells);

        public override void Place(IEnumerable<Character.CharacterCtrl> ctrls, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells) {

            var minPosition = cells.MinElement(c => c.dataPosition.x + c.dataPosition.y);
            var maxPosition = cells.MaxElement(c => c.dataPosition.x + c.dataPosition.y);

            var center = (minPosition.dataPosition + maxPosition.dataPosition) / 2;

            var forIgnore = new List<Cell>();

            var (element, _) = cells.ClosestElement(center,
                (c, centerEle) => Vector2Int.Distance(c.dataPosition, centerEle)
            );

            foreach (var charObj in ctrls) {

                element.StayCtrl(charObj);
                forIgnore.Add(element);

                (element, _) = cells.ClosestElement(
                    center,
                    (c, centerEle) => Vector2Int.Distance(c.dataPosition, centerEle),
                    forIgnore
                );

            }

        }
    }
}
