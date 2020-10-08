using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class StartRoomData : RoomData {

        public StartRoomData(Vector2Int size) : base(size) { }

        public override string GetRoomName() => "StartRoom";

    }

}
