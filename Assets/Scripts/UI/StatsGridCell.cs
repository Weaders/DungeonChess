using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.StarsData;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Observable.TextExtension;

namespace Assets.Scripts.UI {

    public enum CellWhatDisplay {
        Hp, Ad
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
            }

        }

    }

}
