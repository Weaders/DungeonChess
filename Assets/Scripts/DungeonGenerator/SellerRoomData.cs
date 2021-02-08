using System;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Items;
using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.UI.SelectPopup.SelectPanel;

namespace Assets.Scripts.DungeonGenerator {
    public class SellerRoomData : RoomData {

        public SellerRoomData(string titleKey) : base(titleKey) {
        }

        public override object Clone()
            => new SellerRoomData(_title);

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            var items = GameMng.current.gameData.GetRandomItemsPrefabs(3);

            var buyData = new BuyItemData[3];

            for (var i = 0; i < items.Length; i++)
                buyData[i] = new BuyItemData(items[i]);

            GameMng.current.selectPanel.SetItems((buyData[0], buyData[2], buyData[1]), (_) => {
                GameMng.current.cellsGridMng.DisplayExits();
            }, new[] { 
                new FallBackBtn(TranslateReader.GetTranslate("hide"), GameMng.current.selectPanel.Hide),
                new FallBackBtn(TranslateReader.GetTranslate("dont_buy_anything"), () => {
                    GameMng.current.selectPanel.Hide(); 
                    GameMng.current.cellsGridMng.DisplayExits(); 
                })
            });

            GameMng.current.selectPanel.Show();

        }

        public override string GetRoomDescription()
            => TranslateReader.GetTranslate("seller_room_description");

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.sellerRoom.roomSprite;
    }
}
