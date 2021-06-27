using System.Collections.Generic;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public abstract class TeamPlaceStrategy {

        public void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells)
            => Place(team.aliveChars, cells, enemyCells);

        public void Place(IEnumerable<CharacterCtrl> ctrls, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells) {

            var toPlace = CalcPlace(ctrls, cells, enemyCells);

            foreach (var place in toPlace) {
                place.cell.StayCtrl(place.characterCtrl);
            }

        }

        public CtrlToStay[] CalcPlace(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells)
            => CalcPlace(team.aliveChars, cells, enemyCells);

        public abstract CtrlToStay[] CalcPlace(IEnumerable<CharacterCtrl> ctrls, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells);

        public class CtrlToStay {

            public CharacterCtrl characterCtrl;
            public Cell cell;


        }
    }
}
