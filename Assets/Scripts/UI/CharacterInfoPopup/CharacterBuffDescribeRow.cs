using Assets.Scripts.Buffs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharacterInfoPopup {
    public class CharacterBuffDescribeRow : MonoBehaviour {


        [SerializeField]
        private Text title;

        [SerializeField]
        private TextMeshProUGUI description;

        public void SetData(Buff buff) {

            title.text = buff.title;
            description.text = buff.description;

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
