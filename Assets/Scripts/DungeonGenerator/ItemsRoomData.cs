using Assets.Scripts.CellsGrid;

namespace Assets.Scripts.DungeonGenerator {

    public class ItemsRoomData : RoomData {

        public ItemsRoomData(string titleKey) : base(titleKey) { }

        public override object Clone()
            => new ItemsRoomData(_title);

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            var items = GameMng.current.gameData.GetRandomItemsPrefabs(3);

            GameMng.current.selectPanel.SetItems((items[0], items[2], items[1]), (_) => {
                GameMng.current.cellsGridMng.DisplayExits();
            });

            GameMng.current.selectPanel.Show();

        }

    }

}
