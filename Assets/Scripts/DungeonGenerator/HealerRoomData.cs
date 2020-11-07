using Assets.Scripts.ActionsData;
using Assets.Scripts.CellsGrid;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Assets.Scripts.DungeonGenerator {
    public class HealerRoomData : RoomData {

        public HealerRoomData(string titleKey) : base(titleKey) {
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            foreach (var c in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {
                c.characterData.actions.GetHeal(c, new Heal(c.characterData.stats.maxHp.val / 10));
            }

            GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {
                msg = "You has been healed for 10%",
                btnOk = "Ok"
            });

            GameMng.current.messagePanel.Show();

            GameMng.current.cellsGridMng.DisplayExits();

        }
    }
}
