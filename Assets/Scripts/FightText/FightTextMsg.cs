using Assets.Scripts.Logging;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.FightText {

    [ExecuteAlways]
    public class FightTextMsg : MonoBehaviour {

        [SerializeField]
        private Text textObj;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Image img;

        public void SetText(string text, SetTextOpts opts) {

            textObj.text = text;
            textObj.color = opts.color;
            textObj.fontSize = opts.size;

            if (opts.icon != null) {
                img.sprite = opts.icon;
            } else {
                img.gameObject.SetActive(false);
            }

        }

        public class SetTextOpts {

            public Color color;
            public int size = 1;
            public Sprite icon;

        }
    }
}
