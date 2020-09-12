using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Translate {

    public class TranslateText : MonoBehaviour {

        [SerializeField]
        private TMP_Text textToTranslate;

        [SerializeField]
        private Text defaultText;

        private void Reset() {

            textToTranslate = GetComponent<TMP_Text>();
            defaultText = GetComponent<Text>();

        }

        private void Awake() {

            if (textToTranslate != null)
                textToTranslate.text = TranslateReader.GetTranslate(textToTranslate.text);

            if (defaultText != null)
                defaultText.text = TranslateReader.GetTranslate(defaultText.text);

        }

    }
}
