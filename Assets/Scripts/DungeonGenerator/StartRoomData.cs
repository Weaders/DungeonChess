using Assets.Scripts.CellsGrid;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class StartRoomData : EnemyRoomData {

        public StartRoomData(Vector2Int size) : base(size) { }

        public override string GetRoomName() => "StartRoom";

    }

}
