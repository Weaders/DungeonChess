using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {


    public class CellsGridMng : MonoBehaviour {

        public IEnumerable<Cell> enemiesSideCell => _enemiesSideCell;

        public IEnumerable<Cell> playerSideCell => _playerSideCell;

        private List<Cell> _enemiesSideCell = new List<Cell>();

        private List<Cell> _playerSideCell = new List<Cell>();

        private List<GameObject> _exists = new List<GameObject>();

        [SerializeField]
        private ArrowCtrl arrowCtrlPrefab;

        public void Init() {
            GameMng.current.fightMng.onPlayerWin.AddListener(DisplayExits);
        }

        public void RefreshCells() {

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

        public void DisplayExits() {

            foreach (Cell cell in GameMng.current.roomCtrl.currentRoom.GetExits()) {

                var obj = Instantiate(arrowCtrlPrefab.gameObject, cell.transform);
                obj.transform.localPosition = Vector3.zero;

                obj.GetComponent<ArrowCtrl>().onClick.AddListener(
                    () => GameMng.current.roomCtrl.MoveToNextRoom(cell.exitDirection)
                );

                _exists.Add(obj);                

            }

        }

        private void HideExits() {
            foreach (var exit in _exists) {
                Destroy(exit);
            }
        }

        private IEnumerable<Cell> GetCells() => GameMng.current.roomCtrl.currentRoom.GetCells();

    }

}
