﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Data {

    public class MedivleDungeonGenerator : DungeonObjectGenerator {

        public int cellOffset;

        public float roomsOffset = 3f;

        public GameObject cellPrefab;

        public GameObject wallPrefab;

        public GameObject exitPrefab;

        public GameObject wallWithStencilPrefab;

        public GameObject lampPrefab;

        public Vector3 lampPosition;

        public float lampChance = 0.3f;

        private List<DungeonRoomCells> dungeonRooms = new List<DungeonRoomCells>();

        public override void Generate(DungeonDataPosition data) {

            dungeonRooms = new List<DungeonRoomCells>();

            var rooms = new Queue<(Vector3, RoomData, Vector2)>();

            rooms.Enqueue((Vector3.zero, data.startRoom, Vector2.zero));

            while (rooms.Any()) {

                var (currentRoomPos, currentRoom, roomDataPos) = rooms.Dequeue();

                var cells = GenerateFloor(currentRoomPos, currentRoom, roomDataPos);
                GenerateWalls(cells, GetOrCreate(roomDataPos, currentRoom), currentRoom.size);

                if (currentRoom.exitFromRooms != null) {

                    foreach (var exit in currentRoom.exitFromRooms) {

                        var nextRoomPos = currentRoomPos;
                        var nextRoomDataPos = roomDataPos;

                        if (exit.direction == Direction.bottom) {

                            nextRoomPos = new Vector3(
                                currentRoomPos.x,
                                currentRoomPos.y - currentRoom.size.y - roomsOffset,
                                currentRoomPos.z
                            );

                            nextRoomDataPos += new Vector2(0, -1);

                        } else if (exit.direction == Direction.top) {

                            nextRoomPos = new Vector3(
                                currentRoomPos.x,
                                currentRoomPos.y + (roomsOffset + exit.toRoomData.size.y) * cellOffset,
                                currentRoomPos.z
                            );

                            nextRoomDataPos += new Vector2(0, 1);

                        } else if (exit.direction == Direction.left) {

                            nextRoomPos = new Vector3(
                               currentRoomPos.x - (roomsOffset + exit.toRoomData.size.x) * cellOffset,
                               currentRoomPos.y,
                               currentRoomPos.z
                           );

                            nextRoomDataPos += new Vector2(-1, 0);

                        } else if (exit.direction == Direction.right) {

                            nextRoomPos = new Vector3(
                               currentRoomPos.x + currentRoom.size.x + roomsOffset,
                               currentRoomPos.y,
                               currentRoomPos.z
                           );

                            nextRoomDataPos += new Vector2(1, 0);

                        }

                        rooms.Enqueue((nextRoomPos, exit.toRoomData, nextRoomDataPos));

                    }

                }

            }

        }

        [ContextMenu("GenerateBase")]
        public void Generate() {

            var data = new DungeonDataGenerator();

            //GenerateAndBake(
            //    data.GetDungeonDataPositions(GameMng.current.currentDungeonData)
            //);

            Debug.Log($"Rooms clear");

            //GetComponent<CellsGridMng>().rooms = dungeonRooms.ToArray();

        }

        private List<Cell> GenerateFloor(Vector3 leftUpper, RoomData roomData, Vector2 roomDataPosition) {

            var size = roomData.size;
            var exists = roomData.exitFromRooms;

            var roomObj = GetOrCreate(roomDataPosition, roomData);

            var cells = new List<Cell>();

            for (var x = 0; x < size.x; x++) {

                for (var y = 0; y < size.y; y++) {

                    DungeonRoomCells exitForRoom = null;
                    Direction direction = default;

                    if (x == size.x / 2) {

                        if (y == 0) {

                            var bottomExit = exists.FirstOrDefault(e => e.direction == Direction.bottom);

                            if (bottomExit != null) {
                                exitForRoom = GetOrCreate(roomDataPosition + new Vector2(0, -1), bottomExit.toRoomData);
                                direction = Direction.bottom;
                            }

                        } else if (y == size.y - 1) {

                            var topExit = exists.FirstOrDefault(e => e.direction == Direction.top);

                            if (topExit != null) {
                                exitForRoom = GetOrCreate(roomDataPosition + new Vector2(0, 1), topExit.toRoomData);
                                direction = Direction.top;
                            }

                        }

                    } else if (y == size.y / 2) {

                        if (x == 0) {

                            var leftExit = exists.FirstOrDefault(e => e.direction == Direction.left);

                            if (leftExit != null) {
                                exitForRoom = GetOrCreate(roomDataPosition + new Vector2(-1, 0), leftExit.toRoomData);
                                direction = Direction.left;
                            }

                        } else if (x == size.x - 1) {

                            var rigthExit = exists.FirstOrDefault(e => e.direction == Direction.right);

                            if (rigthExit != null) {
                                exitForRoom = GetOrCreate(roomDataPosition + new Vector2(1, 0), rigthExit.toRoomData);
                                direction = Direction.right;
                            }

                        }

                    }

                    var cell = Instantiate(cellPrefab, roomObj.transform);
                    var cellObj = cell.GetComponent<Cell>();

                    if (y >= size.y / 2) {
                        cellObj.SetCellType(Cell.CellType.ForEnemy);
                    } else {
                        cellObj.SetCellType(Cell.CellType.ForPlayer);
                    }

                    cellObj.dataPosition = new Vector2Int(x, y);

                    cells.Add(cellObj);

                    cell.transform.localPosition = new Vector3(
                        leftUpper.x + (x * cellOffset),
                        0,
                        leftUpper.y + (y * cellOffset)
                    );

                }

            }

            return cells;

        }

        private void GenerateWalls(IEnumerable<Cell> cells, DungeonRoomCells roomsCells, Vector2Int size) {

            var wallGroups = new WallGroup[4] { new WallGroup(), new WallGroup(), new WallGroup(), new WallGroup() };

            foreach (var cell in cells) {

                var genLamp = UnityEngine.Random.value < lampChance;

                GameObject wall = null;

                var selectedPrefab = wallPrefab;

                if (cell.IsExit()) {
                    selectedPrefab = exitPrefab;
                }

                if (cell.dataPosition.y == 0) {

                    wall = Instantiate(wallWithStencilPrefab, roomsCells.transform);

                    wallGroups[0].transforms.Add(wall.transform);

                    wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    wall.transform.localPosition = new Vector3(cell.transform.localPosition.x, 0, cell.transform.localPosition.z - 2);

                } else if (cell.dataPosition.y == size.y - 1) {

                    wall = Instantiate(selectedPrefab, roomsCells.transform);

                    wallGroups[1].transforms.Add(wall.transform);

                    wall.transform.localRotation = Quaternion.Euler(0, 180, 0);
                    wall.transform.localPosition = new Vector3(cell.transform.localPosition.x, 0, cell.transform.localPosition.z + 2);

                }

                if (cell.dataPosition.x == 0) {

                    wall = Instantiate(selectedPrefab, roomsCells.transform);

                    wallGroups[2].transforms.Add(wall.transform);

                    wall.transform.localRotation = Quaternion.Euler(0, 90, 0);
                    wall.transform.localPosition = new Vector3(cell.transform.localPosition.x - 2, 0, cell.transform.localPosition.z);

                } else if (cell.dataPosition.x == size.x - 1) {

                    wall = Instantiate(selectedPrefab, roomsCells.transform);

                    wallGroups[3].transforms.Add(wall.transform);

                    wall.transform.localRotation = Quaternion.Euler(0, -90, 0);
                    wall.transform.localPosition = new Vector3(cell.transform.localPosition.x + 2, 0, cell.transform.localPosition.z);

                }

                if (genLamp && wall != null) {

                    var lamp = Instantiate(lampPrefab, wall.transform);
                    lamp.transform.localPosition = lampPosition;
                    lamp.transform.localRotation = Quaternion.identity;

                }

            }

            roomsCells.wallGroups = wallGroups;

        } 

        private DungeonRoomCells GetOrCreate(Vector2 pos, RoomData roomData) {

            var roomObj = dungeonRooms.FirstOrDefault(x => x.dataPosition == pos);

            if (roomObj == null) {

                var gameObj = new GameObject(roomData.GetRoomName());
                gameObj.transform.SetParent(transform);

                roomObj = gameObj.AddComponent<DungeonRoomCells>();
                roomObj.dataPosition = pos;

                dungeonRooms.Add(roomObj);

            }            

            return roomObj;

        }

    }

    [Serializable]
    public class WallGroup {

        public List<Transform> transforms = new List<Transform>();

        public Vector3 centerPos { 
            get {

                if (!transforms.Any())
                    return Vector3.zero;

                return transforms[transforms.Count / 2].position;

            }

        }

    }

}
