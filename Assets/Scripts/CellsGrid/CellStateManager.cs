using System.Collections.Generic;
using System.Linq;
using static Assets.Scripts.CellsGrid.Cell;

namespace Assets.Scripts.CellsGrid {
    public class CellStateManager {

        public SortedSet<CellState> states = new SortedSet<CellState>();

        public CellStateManager() {
            states.Add(default);
        }

        public void AddState(CellState state) {
            states.Add(state);
        }

        public void RemoveState(CellState state) {
            states.Remove(state);
        }

        public bool ContainsState(CellState state) =>
            states.Contains(state);

        public CellState GetCurrent()
            => states.Last();

    }
}
