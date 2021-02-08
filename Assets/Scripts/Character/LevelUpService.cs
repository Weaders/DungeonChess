using System.Linq;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.MessageData;

namespace Assets.Scripts.Character {

    public class LevelUpService : MonoBehaviour {

        [SerializeField]
        private Button button;

        [SerializeField]
        private int cost;

        public void Init() {

            GameMng.current.playerData.money.onPostChange.AddSubscription(Observable.OrderVal.UIUpdate, changeData => {
                button.interactable = changeData.newVal >= cost;
            });

        }

        public void LevelUp() {

            GameMng.current.playerData.money.val -= cost;

            GameMng.current.playerData.levelOfCharacters.val += 1;

            foreach (var buyData in GameMng.current.buyMng.buyDataList) {

                var chars = GameMng.current.fightMng.fightTeamPlayer.aliveChars.Where(b =>
                    b.characterData.id == buyData.ctrlPrefab.characterData.id
                );

                if (!chars.Any())
                    continue;

                var statGroup = buyData.GetStatGroup(GameMng.current.playerData.levelOfCharacters);

                foreach (var character in chars) {

                    character.effectsPlacer.PlaceEffect(GameMng.current.gameData.onGetGoodEffect.gameObject);

                    foreach (var stat in statGroup.stats)
                        character.characterData.stats.Mofify(
                            stat,
                            statGroup.modifyType
                        );

                }
                    

            }

        }

        public void LevelUpToCurrent(CharacterCtrl ctrl) {

            foreach (var buyData in GameMng.current.buyMng.buyDataList) {

                if (ctrl.characterData.id != buyData.ctrlPrefab.characterData.id)
                    continue;

                var statGroup = buyData.GetStatGroup(GameMng.current.playerData.levelOfCharacters);

                var countOfPlus = 0;

                if (statGroup.levelOfDifficult == -1) {

                    var maxStatGroup =
                        buyData.GetStatGroup(GameMng.current.playerData.levelOfCharacters, true);

                    if (maxStatGroup.levelOfDifficult != -1) {

                        foreach (var stat in maxStatGroup.stats)
                            ctrl.characterData.stats.Mofify(
                                stat,
                                Observable.ModifyType.Set
                            );

                        countOfPlus = GameMng.current.playerData.levelOfCharacters - maxStatGroup.levelOfDifficult;

                    } else {

                        countOfPlus = GameMng.current.playerData.levelOfCharacters;

                    }

                    foreach (var stat in statGroup.stats) {

                        for (var i = 0; i < countOfPlus; i++) {

                            ctrl.characterData.stats.Mofify(
                                stat,
                                Observable.ModifyType.Plus
                            );

                        }

                    }

                } else {

                    foreach (var stat in statGroup.stats) {

                        ctrl.characterData.stats.Mofify(
                            stat,
                            Observable.ModifyType.Set
                        );

                    }

                }

                if (GameMng.current.playerData.levelOfCharacters > 0) {
                    ctrl.effectsPlacer.PlaceEffect(GameMng.current.gameData.onGetGoodEffect.gameObject);
                }

            }

        }

        public void ShowLevelUp() {

            GameMng.current.messagePanel.SetData(new UI.MessagePopup.MessagePanel.MessageData {

                msg = TranslateReader.GetTranslate("you_want_level_up", new Placeholder("cost", cost)),
                btns = new BtnData[]
                {
                    new BtnData(TranslateReader.GetTranslate("yes"), () => { 
                        
                        LevelUp();
                        GameMng.current.messagePanel.Hide(); 

                    }),
                    new BtnData(TranslateReader.GetTranslate("no"), GameMng.current.messagePanel.Hide)
                }
            });

            GameMng.current.messagePanel.Show();

        }

    }
}
