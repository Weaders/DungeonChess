using Assets.Scripts.BuyMng;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharacterBuyPanel {

    public class CharacterBuyPanelBtn : Button {

        [SerializeField]
        private Text characterTitle;
        
        [SerializeField]
        private Text characterDescription;

        [SerializeField]
        private Text cost;

        public void SetBuyData(BuyData buyData) {

            characterTitle.text = TranslateReader.GetTranslate(buyData.characterCtrl.characterData.characterName);
            characterDescription.text = TranslateReader.GetTranslate(buyData.descriptionKey);
            cost.text = buyData.cost.ToString();

        }

        public bool IsSelected()
            => currentSelectionState == SelectionState.Selected;

    }

}
