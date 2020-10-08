using UnityEngine;

namespace Assets.Scripts.DungeonGenerator
{
	public class BossRoomData : RoomData {

        public BossRoomData(Vector2Int s) : base(s) { }

        public override string GetRoomName() => "BossRoom";

    }

}
