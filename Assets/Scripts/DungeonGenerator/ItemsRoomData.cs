﻿using Assets.Scripts.CellsGrid;
using Assets.Scripts.Translate;
using UnityEngine;

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

        public override string GetRoomDescription()
            => TranslateReader.GetTranslate("items_room_description");

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.itemsRoom.roomSprite;
    }

}
