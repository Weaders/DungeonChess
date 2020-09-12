using System.Numerics;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class DungeonDataGenerator {

        public DungeonDataPosition GetDungeonData() {

            var startRoom = new RoomData(new Vector2Int(6,6));

            var secondRoom = new RoomData(new Vector2Int(8, 8));

            startRoom.AddExit(secondRoom, Direction.top);

            return new DungeonDataPosition(new[] { startRoom, secondRoom });

        }

    }

}
