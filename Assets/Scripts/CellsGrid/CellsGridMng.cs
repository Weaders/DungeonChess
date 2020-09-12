using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {
    public class CellsGridMng : MonoBehaviour {

        public IEnumerable<Cell> enemiesSideCell => _enemiesSideCell;

        public IEnumerable<Cell> playerSideCell => _playerSideCell;

        private List<Cell> _enemiesSideCell = new List<Cell>();

        private List<Cell> _playerSideCell = new List<Cell>(); 

        [SerializeField]
        private ArrowCtrl arrowCtrlPrefab;

        [SerializeField]
        private DungeonRoomCells currentRoomCells;

        public void Init() {

            GameMng.current.fightMng.onPlayerWin.AddListener(DisplayExits);
            RefreshCells();

        }

        private void RefreshCells() {

            _enemiesSideCell = new List<Cell>();
            _playerSideCell = new List<Cell>();

            foreach (Cell cell in GetCells()) {

                if (cell.GetCellType() == Cell.CellType.ForEnemy) {
                    _enemiesSideCell.Add(cell);
                } else {
                    _playerSideCell.Add(cell);
                }
            }

        }

        public void MoveTo(DungeonRoomCells dungeonRoomCells) {

            currentRoomCells = dungeonRoomCells;

            var pos = dungeonRoomCells.GetCenterPosition();

            Camera.main.transform.position = new Vector3(pos.x, Camera.main.transform.position.y, pos.z);

            RefreshCells();
        }

        private void DisplayExits() {

            foreach (Cell cell in GetCells()) {

                if (cell.IsExit()) {
                    
                    var obj = Instantiate(arrowCtrlPrefab, cell.transform);
                    obj.transform.localPosition = Vector3.zero;

                    obj.GetComponent<ArrowCtrl>().onClick.AddListener(() => MoveTo(cell.GetExit()));

                }

            }

        }

        private IEnumerable<Cell> GetCells() => currentRoomCells.GetCells();
    }

}
