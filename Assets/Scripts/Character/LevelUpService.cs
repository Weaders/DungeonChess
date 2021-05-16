using System.Linq;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.UI.MessagePopup.MessagePanel.BaseMessageData;
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
                        character.characterData.stats.Modify(
                            stat,
                            statGroup.modifyType
                        );

                }
                    

            }

        }

        public void LevelUpToCurrent(CharacterCtrl ctrl, bool isEnemy = false) {

            var levels = !isEnemy ?
                    GameMng.current.playerData.levelOfCharacters : GameMng.current.levelDifficult;

            CharacterDungeonData characterData = null;

            if (isEnemy) {

                var enemiesTeams = GameMng.current.currentDungeonData.enemiesPoll.teams;

                characterData = enemiesTeams.SelectMany(t => t.characterCtrls).First(c =>
                    c.characterCtrl.characterData.id == ctrl.characterData.id
                );

            } else {

                characterData = GameMng.current.buyMng.buyDataList
                    .First(b => b.ctrlPrefab.characterData.id == ctrl.characterData.id);

            }

            var statGroup = characterData.GetStatGroup(levels);

            var countOfPlus = 0;

            if (statGroup.levelOfDifficult == -1) {

                var maxStatGroup =
                    characterData.GetStatGroup(levels, true);

                if (maxStatGroup.levelOfDifficult != -1) {

                    foreach (var stat in maxStatGroup.stats)
                        ctrl.characterData.stats.Modify(
                            stat,
                            Observable.ModifyType.Set
                        );

                    countOfPlus = levels - maxStatGroup.levelOfDifficult;

                } else {

                    countOfPlus = levels;

                }

                foreach (var stat in statGroup.stats) {

                    for (var i = 0; i < countOfPlus; i++) {

                        ctrl.characterData.stats.Modify(
                            stat,
                            Observable.ModifyType.Plus
                        );

                    }

                }

            } else {

                foreach (var stat in statGroup.stats) {

                    ctrl.characterData.stats.Modify(
                        stat,
                        Observable.ModifyType.Set
                    );

                }

            }

            if (levels > 0) {
                ctrl.effectsPlacer.PlaceEffect(GameMng.current.gameData.onGetGoodEffect.gameObject);
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
