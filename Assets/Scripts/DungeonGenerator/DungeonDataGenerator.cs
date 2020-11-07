using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    public class DungeonDataGenerator {

        public DungeonDataPosition GetDungeonDataPositions(DungeonData dungeonData) {

            var countRooms = Random.Range(dungeonData.countRooms.min, dungeonData.countRooms.max);

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

                    var countExists = Random.Range(1, 3);

                    for (var e = 0; e < directionsExits.Length; e++) {

                        if (!node.data.IsThereExit(directionsExits[e])) {

                            //new RoomData(roomSize)
                            RoomData newRoom = null;

                            node.data.AddExit(newRoom, directionsExits[e]);

                            var neweNode = node.AddChild(newRoom);
                            roomsStack.Push(neweNode);

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
