using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.FightText {

    public class FightTextMsg : MonoBehaviour {

        [SerializeField]
        private Text textObj;

        [SerializeField]
        private float speed;

        public void SetText(string text, Color color) {

            textObj.text = text;
            textObj.color = color;

        }

        private void Update() {
            transform.Translate(transform.up * speed * Time.deltaTime);
        }


    }
}
