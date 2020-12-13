using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Translate;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

namespace Assets.Scripts.DungeonGenerator {
    public class RerollRoomData : RoomData {
        public RerollRoomData(string titleKey) : base(titleKey) {
        }

        public override object Clone()
            => new RerollRoomData(_title);

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {
                msg = TranslateReader.GetTranslate("want_reroll_hero"),
                btns = new[] {
                    new BtnData {
                        title = TranslateReader.GetTranslate("yes"),
                        onClick = () => {
                            GameMng.current.messagePanel.Hide();
                            GameMng.current.rerollCtrl.Activate();
                        }
                    },
                    new BtnData {
                        title = TranslateReader.GetTranslate("no"),
                        onClick = () => {

                            GameMng.current.messagePanel.Hide();
                            GameMng.current.cellsGridMng.DisplayExits();

                        }
                    }
                }
            });

            GameMng.current.messagePanel.Show();

        }
    }
}
