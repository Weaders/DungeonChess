using Assets.Scripts.CellsGrid;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
	public class BossRoomData : RoomData {

        public BossRoomData(Vector2Int s) : base(s) { }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {
            //throw new System.NotImplementedException();
        }

        public override string GetRoomName() => "BossRoom";

    }

}
