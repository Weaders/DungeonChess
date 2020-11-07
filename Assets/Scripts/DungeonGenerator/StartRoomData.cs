using Assets.Scripts.CellsGrid;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class StartRoomData : EnemyRoomData {

        public StartRoomData(string titleKey) : base(titleKey) { }

        public override string GetRoomName() => "StartRoom";

    }

}
