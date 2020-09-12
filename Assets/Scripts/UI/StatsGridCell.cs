using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

    public enum CellWhatDisplay {
        Hp, Ad
    }

    public class StatsGridCell : MonoBehaviour {

        [SerializeField]
        private Text contentText;
        
        private SubsribeTextResult subsribeTextResult;

        public void SetCharacter(CharacterData data, CellWhatDisplay whatDisplay) {

            if (subsribeTextResult != null)
                subsribeTextResult.Unsubscribe();

            switch (whatDisplay) {
                case CellWhatDisplay.Hp:
                    subsribeTextResult = contentText.Subscribe(
                        () => $"{data.stats.hp}/{data.stats.maxHp}",
                        OrderVal.UIUpdate,
                        data.stats.hp,
                        data.stats.maxHp
                    );
                    break;
                case CellWhatDisplay.Ad:
                    subsribeTextResult = contentText.Subscribe(
                        data.stats.AD,
                        OrderVal.UIUpdate
                    );
                    break;
            }

        }


    }
}
