using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Translate;
using UnityEngine.Events;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

namespace Assets.Scripts.Player {
    public class RerollCtrl {

        public UnityEvent onPostReroll = new UnityEvent();

        public void Activate() {

            GameMng.current.locationTitle.SetData(new UI.GameTitlePopup.GameTitlePanel.SetDataMsg {
                text = TranslateReader.GetTranslate("select_hero_for_reroll")
            });

            GameMng.current.gameInputCtrl.onChangeSelectCharacter.AddListener(OnSelectCharacter);

        }

        public void OnSelectCharacter(CharacterCtrl old, CharacterCtrl ctrl) {

            if (ctrl != null) {

                GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {
                    msg = TranslateReader.GetTranslate("change_character_to_random"),
                    btns = new[] {
                    new BtnData {
                        title = TranslateReader.GetTranslate("yes"),
                        onClick = () => {

                            RerollCharacter(ctrl);
                            GameMng.current.messagePanel.Hide();

                        }
                    },
                    new BtnData {
                        title = TranslateReader.GetTranslate("no"),
                        onClick = () => {
                            GameMng.current.messagePanel.Hide();
                        }
                    }
                }
                });

                GameMng.current.messagePanel.Show();

            }

        }

        public CharacterCtrl RerollCharacter(CharacterCtrl characterCtrl) {

            var team = GameMng.current.fightMng.GetTeamFor(characterCtrl);

            if (team != null) {

                var randomCtrl = GameMng.current.buyMng.characterCtrls.RandomElement();

                var cellForCharacter = characterCtrl.cell;

                var addedCtrl = team.AddCharacterToTeamPrefab(randomCtrl);

                cellForCharacter.StayCtrl(addedCtrl);

                for (var i = 0; i < characterCtrl.characterData.itemsContainer.Count; i++) {
                    addedCtrl.characterData.itemsContainer[i] = characterCtrl.characterData.itemsContainer[i];
                }

                team.RemoveCharacter(characterCtrl);

                onPostReroll.Invoke();

                return addedCtrl;

            }

            return null;

        }

        public void Deactivate() {

            GameMng.current.gameInputCtrl.onChangeSelectCharacter.RemoveListener(OnSelectCharacter);

            GameMng.current.locationTitle.HidePopup();

        }

    }
}
