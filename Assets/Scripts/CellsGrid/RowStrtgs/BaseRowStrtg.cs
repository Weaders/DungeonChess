using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CellsGrid.RowStrtgs {

    public interface IRowStrtg {
        IEnumerable<Cell> CalcCellsFor(Cell cell);
    }

    public abstract class BaseRowStrtg : MonoBehaviour, IRowStrtg {

        public abstract IEnumerable<Cell> CalcCellsFor(Cell cell);
    }
}
