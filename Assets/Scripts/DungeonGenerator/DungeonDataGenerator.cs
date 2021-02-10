using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    [Obsolete]
    public class DungeonDataGenerator {

        public DungeonDataPosition GetDungeonDataPositions(DungeonData dungeonData) {

            var countRooms = UnityEngine.Random.Range(dungeonData.changeDifficultEvery.min, dungeonData.changeDifficultEvery.max);

            //var roomSize = new Vector2Int(6, 6);

            var directionsExits = new[] { Direction.top, Direction.left, Direction.right };

            var startRoom = new TreeNode<RoomData>(new StartRoomData("one_more_room"));

            var rooms = new List<RoomData>();

            var roomsStack = new Stack<TreeNode<RoomData>>();

            roomsStack.Push(startRoom);

            rooms.Add(startRoom.data);

            while (roomsStack.Any()) {

                var node = roomsStack.Pop();

                if (node.level == countRooms) {

                    var newRoom = new BossRoomData("one_more_room");

                    node.data.AddExit(newRoom, Direction.top);

                    rooms.Add(newRoom);

                } else {

                    var countExists = UnityEngine.Random.Range(2, 3);

                    TagLogger<DungeonDataGenerator>.Info($"For {rooms.Count - 1} will be generated {countExists} exists");

                    for (var e = 0; e < directionsExits.Length; e++) {

                        if (!node.data.IsThereExit(directionsExits[e])) {

                            RoomData newRoom = null;

                            node.data.AddExit(newRoom, directionsExits[e]);

                            var newNode = node.AddChild(newRoom);
                            roomsStack.Push(newNode);

                            rooms.Add(newRoom);

                        }

                        if (node.data.exitFromRooms.Count == countExists)
                            break;

                    }

                }

            }

            return new DungeonDataPosition(rooms);

        }

    }

}
