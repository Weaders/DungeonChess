using Assets.Scripts.CellsGrid;
using Assets.Scripts.Translate;
using UnityEngine;
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
                            GameMng.current.rerollCtrl.onPostReroll.AddListener(OnPostReroll);

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

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.rerollRoom.roomSprite;

        public override string GetRoomDescription()
            => TranslateReader.GetTranslate("reroll_room_description");

        public void OnPostReroll() {

            GameMng.current.rerollCtrl.Deactivate();
            GameMng.current.rerollCtrl.onPostReroll.RemoveListener(OnPostReroll);
            GameMng.current.cellsGridMng.DisplayExits();

        }
    }
}
