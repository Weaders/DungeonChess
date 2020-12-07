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

        public void SetText(string text, SetTextOpts opts) {

            textObj.text = text;
            textObj.color = opts.color;
            textObj.fontSize = opts.size;

        }

        public class SetTextOpts {
            public Color color;
            public int size = 1;
        }
    }
}
