﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.DungeonGenerator;
using Assets.Scripts.Logging;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.CellsGrid {


    public class CellsGridMng : MonoBehaviour {

        public IEnumerable<Cell> enemiesSideCell => _enemiesSideCell;

        public IEnumerable<Cell> playerSideCell => _playerSideCell;

        private List<Cell> _enemiesSideCell = new List<Cell>();

        private List<Cell> _playerSideCell = new List<Cell>();

        private Dictionary<Vector2Int, Cell> cellsByPlace = new Dictionary<Vector2Int, Cell>();

        private List<GameObject> _exists = new List<GameObject>();

        [SerializeField]
        private ArrowCtrl arrowCtrlPrefab;

        public void Init() {
            //GameMng.current.fightMng.onPlayerWin.AddListener(DisplayExits);
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

                cellsByPlace.Add(cell.dataPosition, cell);

            }

        }

        public void DisplayExits() {

            var exists = GameMng.current.roomCtrl.currentRoom.roomData.exitFromRooms
                .Take(3)
                .ToArray();

            TagLogger<CellsGridMng>.Info($"Display {exists.Length} exists");

            ExitFromRoom getExit(int i)  {
                if (exists.Length > i) {
                    return exists[i];
                } else {
                    return null;
                }
            };

            GameMng.current.selectPanel.titleText = TranslateReader.GetTranslate("select_exit");
            GameMng.current.selectPanel.SetItems((getExit(0), getExit(1), getExit(2)));
            GameMng.current.selectPanel.Show();

            //foreach (Cell cell in GameMng.current.roomCtrl.currentRoom.GetExits()) {

            //    i++;

            //    //var obj = Instantiate(arrowCtrlPrefab.gameObject, cell.transform);
            //    //obj.transform.localPosition = Vector3.zero;

            //    //obj.GetComponent<ArrowCtrl>().onClick.AddListener(
            //    //    () => GameMng.current.roomCtrl.MoveToNextRoom(cell.exitDirection)
            //    //);

            //    //_exists.Add(obj);            

            //}

        }

        private void HideExits() {
            foreach (var exit in _exists) {
                Destroy(exit);
            }
        }

        private IEnumerable<Cell> GetCells() => GameMng.current.roomCtrl.currentRoom.GetCells();

        public IEnumerable<Cell> GetNeighbours(Cell cell, int range = 1) {

            var result = new List<Cell>(range * 4);

            for (var i = 1; i <= range; i++) {

                var offsets = new[] { 
                    new Vector2Int(i, 0),
                    new Vector2Int(0, i),
                    new Vector2Int(-i, 0),
                    new Vector2Int(0, -i)
                };

                foreach (var offset in offsets) {

                    if (cellsByPlace.TryGetValue(cell.dataPosition + offset, out var val)) {
                        result.Add(cellsByPlace[cell.dataPosition + offset]);
                    }

                }

            }

            return result;

        }
    }

}
