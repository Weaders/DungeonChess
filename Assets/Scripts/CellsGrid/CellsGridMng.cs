using System;
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

        public void RefreshCells() {

            _enemiesSideCell = new List<Cell>();
            _playerSideCell = new List<Cell>();

            cellsByPlace.Clear();

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

            ExitFromRoom getExit(int i) {
                if (exists.Length > i) {
                    return exists[i];
                } else {
                    return null;
                }
            };

            GameMng.current.selectPanel.titleText = TranslateReader.GetTranslate("select_exit");
            GameMng.current.selectPanel.SetItems((getExit(0), getExit(1), getExit(2)));
            GameMng.current.selectPanel.Show();

        }

        public void DisplayExitsPopup() {
            GameMng.current.selectPanel.Show();
        }

        public void HideExistsPopup() {
            GameMng.current.selectPanel.Hide();
        }

        private IEnumerable<Cell> GetCells(bool isAvailableForMoveOnly = true, bool onlyUsable = false, bool onlyForPlayer = false) {

            var result = GameMng.current.roomCtrl.currentRoom.GetCells();

            if (isAvailableForMoveOnly)
                result = result.Where(c => c.IsAvailableToStay());

            if (onlyUsable)
                result = result.Where(c => c.GetCellType() != Cell.CellType.NotUsable);

            if (onlyForPlayer)
                result = result.Where(c => c.GetCellType() == Cell.CellType.ForPlayer);

            return result;

        }

        public Vector2Int minXYDataPosition
            => new Vector2Int(GetCells().Min(c => c.dataPosition.x), GetCells().Min(c => c.dataPosition.y));

        public Vector2Int maxXYDataPosition
            => new Vector2Int(GetCells().Max(c => c.dataPosition.x), GetCells().Max(c => c.dataPosition.y));

        public int widthDataPosition
            => GetCells().Max(c => c.dataPosition.x) - GetCells().Min(c => c.dataPosition.x);

        public IEnumerable<Cell> GetByDataPositions(IEnumerable<Vector2Int> positions, bool onlyUsable = false, bool onlyPlayers = true) 
            => GetCells(false, onlyUsable, onlyPlayers).Where(c => positions.Contains(c.dataPosition));

        public IEnumerable<Cell> GetNeighbours(Cell cell, int range = 1, bool isAvailableForMoveOnly = true) {

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

                        if ((isAvailableForMoveOnly && val.IsAvailableToStay()) || !isAvailableForMoveOnly) {
                            result.Add(cellsByPlace[cell.dataPosition + offset]);
                        }

                    }

                }

            }

            return result;

        }
    }

}
