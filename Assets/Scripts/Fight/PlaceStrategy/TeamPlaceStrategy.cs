using System.Collections.Generic;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public abstract class TeamPlaceStrategy {
        
        public abstract void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells);

        public abstract void Place(IEnumerable<CharacterCtrl> ctrls, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells);

    }
}
