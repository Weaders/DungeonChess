using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.NewsPopup {
    public class NewsRow : MonoBehaviour {

        [SerializeField]
        private Text newsRowText;

        [SerializeField]
        private Text newsRowHeader;

        public void SetText(string header, string text) {

            newsRowHeader.text = header;
            newsRowText.text = text;

        }

        public void RecalcHeight() {

            var newHeight = newsRowHeader.preferredHeight;
            newsRowHeader.rectTransform.sizeDelta = new Vector2(
                newsRowHeader.rectTransform.sizeDelta.x,
                newHeight
            );

            newsRowText.rectTransform.localPosition = new Vector3(
                newsRowText.rectTransform.localPosition.x,
                 newHeight,
                 newsRowText.rectTransform.localPosition.z
            );

            var newTextHeight = newsRowText.preferredHeight;
            newsRowText.rectTransform.sizeDelta = new Vector2(
                newsRowText.rectTransform.sizeDelta.x,
                newTextHeight
            );

            var rectTransform = (transform as RectTransform);

            rectTransform.sizeDelta = new Vector2(
                rectTransform.sizeDelta.x,
                newTextHeight + newHeight
            );

        }

    }

}
