using Assets.Scripts.CellsGrid;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class EventRoomData : RoomData {

        public EventRoomData(string titleKey) : base(titleKey) {
        }

        public override object Clone()
            => new EventRoomData(_title);

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {
            GameMng.current.currentDungeonData.eventsPoll.GetRandomEvent().startStage.Exec();
        }

        public override string GetRoomDescription() {
            return TranslateReader.GetTranslate("event_room_description");
        }

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.eventRoom.roomSprite;

    }
}
