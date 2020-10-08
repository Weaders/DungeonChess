using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    public class DungeonDataPosition {

        public RoomData startRoom => rooms.First();

        public IEnumerable<RoomData> rooms { get; }

        public DungeonDataPosition(IEnumerable<RoomData> rooms) {
            this.rooms = rooms;
        }

    }

    public class RoomData {

        public RoomData(Vector2Int sizeData) {
            size = sizeData;
        }

        public Vector2Int size { get; }

        public IReadOnlyList<ExitFromRoom> exitFromRooms => _exitFromRooms;

        public List<ExitFromRoom> _exitFromRooms = new List<ExitFromRoom>();

        public void AddExit(RoomData roomData, Direction direction) {
            _exitFromRooms.Add(new ExitFromRoom(this, roomData, direction));
        }

        public bool IsThereExit(Direction exit)
            => exitFromRooms.Any(e => e.direction == exit);

        public virtual string GetRoomName() => "RoomData";

    }

    public enum Direction {
        top, left, right, bottom
    }

    public class ExitFromRoom {

        public ExitFromRoom(RoomData from, RoomData to, Direction dir) {

            fromRoomData = from;
            toRoomData = to;
            direction = dir;

        }

        public readonly RoomData fromRoomData;

        public readonly RoomData toRoomData;

        public readonly Direction direction;

    }

}
