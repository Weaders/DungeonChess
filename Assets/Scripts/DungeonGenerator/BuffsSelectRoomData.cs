using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Buffs;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class BuffsSelectRoomData : RoomData {

        public BuffsSelectRoomData(string titleKey) : base(titleKey) {
            isShowBuyPanel = true;
        }

        public override object Clone() {
            return new BuffsSelectRoomData(_title);
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            foreach (var ctrl in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {
                ctrl.characterData.actions.GetHeal(ctrl, new Heal(ctrl.characterData.stats.maxHp.val / 10));
            }

            var items = new List<BuffSelectBtn>(3);

            for (var i = 0; i < 3; i++)
                items.Add(new BuffSelectBtn(GameMng.current.currentDungeonData.buffsPoll.buffs.RandomElement(items.Select(x => x.buff))));

            GameMng.current.selectPanel.SetItems((items[0], items[1], items[2]), (sel) => {
                GameMng.current.cellsGridMng.DisplayExits();
            });

            GameMng.current.selectPanel.Show();

        }

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.enemyRoom.roomSprite;

        public override string GetRoomDescription()
            => TranslateReader.GetTranslate("enemies_room_description");
    }
}
