using System;
using System.Collections.Generic;
using Assets.Scripts.CameraMng;
using Assets.Scripts.Common;
using UnityEditor;
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

        [SerializeField]
        private DungeonRoomCells currentRoomCells;

        public DungeonRoomCells currentRoom => currentRoomCells;

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

            HideExits();

            currentRoomCells = dungeonRoomCells;

            Camera.main.GetComponent<CameraCtrl>().ToRoom();

            RefreshCells();

            GameMng.current.fightMng.RefreshEnemies();
            GameMng.current.fightMng.MovePlayerCtrls();

        }

        private void DisplayExits() {

            foreach (Cell cell in GetCells()) {

                if (cell.IsExit()) {
                    
                    var obj = Instantiate(arrowCtrlPrefab.gameObject, cell.transform);
                    obj.transform.localPosition = Vector3.zero;

                    obj.GetComponent<ArrowCtrl>().onClick.AddListener(() => MoveTo(cell.GetExit()));

                    _exists.Add(obj);

                }

            }

        }

        private void HideExits() {
            foreach (var exit in _exists) {
                Destroy(exit);
            }
        }

        private IEnumerable<Cell> GetCells() => currentRoomCells.GetCells();
    }

}
