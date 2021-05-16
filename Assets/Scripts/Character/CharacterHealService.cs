using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.BaseMessageData;

namespace Assets.Scripts.Character {
    public class CharacterHealService : MonoBehaviour {

        [SerializeField]
        private Button btn;

        [SerializeField]
        private int foodCount;

        private void Reset() {
            btn = GetComponent<Button>();
        }

        private void Awake() {
            btn.interactable = false;
        }

        public void Init() {

            GameMng.current.gameInputCtrl.onChangeSelectCharacter.AddListener((from, to) => {
                btn.interactable = IsBtnIneractable();
            });

            GameMng.current.fightMng.isInFight.onPostChange.AddSubscription(Observable.OrderVal.UIUpdate, () => {
                btn.interactable = IsBtnIneractable();
            });

            GameMng.current.playerData.money.onPostChange.AddSubscription(Observable.OrderVal.UIUpdate, () => {
                btn.interactable = IsBtnIneractable();
            });

            btn.onClick.AddListener(() => {

                GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {
                    msg = TranslateReader.GetTranslate("heal_full", new Placeholder("money", foodCount)),
                    btns = new[] {
                        new BtnData {
                            title = TranslateReader.GetTranslate("yes"),
                            onClick = () => {

                                var selectedCharacterCtrl = GameMng.current.gameInputCtrl.selectedCharacterCtrl;

                                GameMng.current.playerData.food.val -= foodCount;

                                selectedCharacterCtrl.characterData.actions.GetHeal(null, new ActionsData.Heal(selectedCharacterCtrl.characterData.stats.maxHp));
                                GameMng.current.messagePanel.Hide();

                            }
                        },
                        new BtnData
                        {
                            title = TranslateReader.GetTranslate("no"),
                            onClick = () => {
                                GameMng.current.messagePanel.Hide();
                            }
                        }
                    }
                });

                GameMng.current.messagePanel.Show();

            });

        }

        private bool IsBtnIneractable()
            => !GameMng.current.fightMng.isInFight
                       && GameMng.current.gameInputCtrl.selectedCharacterCtrl != null
                       && GameMng.current.playerData.money >= foodCount
                       && GameMng.current.gameInputCtrl.selectedCharacterCtrl.teamSide == Fight.TeamSide.Player;

    }
}
