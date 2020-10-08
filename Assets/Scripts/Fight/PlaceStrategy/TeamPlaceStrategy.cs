using System.Collections.Generic;
using Assets.Scripts.CellsGrid;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public abstract class TeamPlaceStrategy {
        public abstract void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells);
    }
}
