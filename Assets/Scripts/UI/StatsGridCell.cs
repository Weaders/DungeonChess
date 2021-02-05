using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.StatsData;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Observable.TextExtension;

namespace Assets.Scripts.UI {

    public enum CellWhatDisplay {
        Hp, Ad, As, Crit, Vamp, ManaPerAttack
    }

    public class StatsGridCell : MonoBehaviour {

        [SerializeField]
        private Image img;

        [SerializeField]
        private Text contentText;
        
        private SubsribeTextResult subsribeTextResult;

        public void SetCharacter(CharacterData data, CellWhatDisplay whatDisplay) {

            if (subsribeTextResult != null) {
                
                subsribeTextResult.Unsubscribe();
                subsribeTextResult = null;

            }

            switch (whatDisplay) {
                case CellWhatDisplay.Hp:
                    subsribeTextResult = contentText.SubscribeStatWithMaxVal(OrderVal.UIUpdate,
                        data.stats.hp,
                        data.stats.maxHp
                    );
                    img.sprite = GameMng.current.gameData.statsInfoData[Stat.Hp].sprite;
                    break;
                case CellWhatDisplay.Ad:
                    subsribeTextResult = contentText.Subscribe(
                        data.stats.AD,
                        OrderVal.UIUpdate
                    );
                    img.sprite = GameMng.current.gameData.statsInfoData[Stat.Ad].sprite;
                    break;
                case CellWhatDisplay.As:
                    subsribeTextResult = contentText.Subscribe(
                        () => $"{data.stats.AS}%",
                        OrderVal.UIUpdate,
                        data.stats.AS                        
                    );
                    img.sprite = GameMng.current.gameData.statsInfoData[Stat.As].sprite;
                    break;
                case CellWhatDisplay.Vamp:
                    subsribeTextResult = contentText.Subscribe(
                        () => $"{data.stats.vampirism}%",
                        OrderVal.UIUpdate,
                        data.stats.vampirism
                    );
                    img.sprite = GameMng.current.gameData.statsInfoData[Stat.Vampirism].sprite;
                    break;
                case CellWhatDisplay.Crit:
                    subsribeTextResult = contentText.Subscribe(
                        () => $"{data.stats.critChance}%",
                        OrderVal.UIUpdate,
                        data.stats.critChance
                    );
                    img.sprite = GameMng.current.gameData.statsInfoData[Stat.CritChance].sprite;
                    break;
                case CellWhatDisplay.ManaPerAttack:
                    subsribeTextResult = contentText.Subscribe(
                        data.stats.manaPerAttack,
                        OrderVal.UIUpdate
                    );
                    img.sprite = GameMng.current.gameData.statsInfoData[Stat.ManaPerAttack].sprite;
                    break;
            }

        }

        private void OnDestroy() {

            if (subsribeTextResult != null) {

                subsribeTextResult.Unsubscribe();
                subsribeTextResult = null;

            }

        }

    }

}
