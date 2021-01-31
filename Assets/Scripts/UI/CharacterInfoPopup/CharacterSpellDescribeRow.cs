using Assets.Scripts.Character;
using Assets.Scripts.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharacterInfoPopup {

    public class CharacterSpellDescribeRow : MonoBehaviour {

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text description;

        public void SetData(Spell spell, CharacterCtrl owner) {

            title.text = spell.GetTitle(owner.characterData);
            description.text = spell.GetDescription(owner.characterData);

        }

    }
}
