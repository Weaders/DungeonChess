﻿using Assets.Scripts.ActionsData;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class HealerRoomData : RoomData {

        public HealerRoomData(string titleKey) : base(titleKey) {
        }

        public override object Clone()
            => new HealerRoomData(_title);

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            foreach (var c in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {
                c.characterData.actions.GetHeal(c, new Heal(c.characterData.stats.maxHp.val / 10));
            }

            GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {
                msg = TranslateReader.GetTranslate("you_healed_for_10"),
                btns = new[] {
                    new UI.MessagePopup.MessagePanel.MessageData.BtnData {
                        title = TranslateReader.GetTranslate("ok"),
                        onClick = () => {

                            GameMng.current.messagePanel.Hide();
                            GameMng.current.cellsGridMng.DisplayExits();

                        }
                    }
                }
            });

            GameMng.current.messagePanel.Show();

        }

        public override string GetRoomDescription()
            => TranslateReader.GetTranslate("healer_room_description");

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.healerRoom.roomSprite;
    }
}
