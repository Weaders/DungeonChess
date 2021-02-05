using Assets.Scripts.CellsGrid;
using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

namespace Assets.Scripts.DungeonGenerator {
    public class FakeItemsRoom : ItemsRoomData {

        public FakeItemsRoom(string titleKey) : base(titleKey) {
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            foreach (var character in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {

                character.characterData.actions.GetDmg(null, new ActionsData.Dmg(
                    Mathf.RoundToInt(character.characterData.stats.hp * .1f),
                    null
                ));

            }

            GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {
                msg = TranslateReader.GetTranslate("treasure_with_trap"),
                btns = new[] {
                    new BtnData(){
                        title = TranslateReader.GetTranslate("ok") + " =(",
                        onClick = () => {
                            GameMng.current.messagePanel.Hide();
                            GameMng.current.cellsGridMng.DisplayExits();
                        }
                    }
                }
            });

            GameMng.current.messagePanel.Show();

        }

        public override object Clone() {
            return new FakeItemsRoom(_title);
        }

    }
}
