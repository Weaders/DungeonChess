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

        public void RecalcHeight() {

            var rect = (description.transform as RectTransform);

            var oldHeight = rect.sizeDelta.y;

            var delta = description.preferredHeight - oldHeight;

            rect.sizeDelta = new Vector2(rect.sizeDelta.x, description.preferredHeight);

            var parentRect = (transform as RectTransform);

            parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, parentRect.sizeDelta.y + delta);

        }

    }
}
