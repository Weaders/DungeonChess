using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.FightText {

    public class FightTextMng : MonoBehaviour {

        [SerializeField]
        private GameObject msgPrefab;

        public void DisplayText(CharacterCtrl ctrl, string text, Color color) {

            var msg = Instantiate(msgPrefab, ctrl.characterCanvas.transform);
            var textMsg = msg.GetComponent<FightTextMsg>();

            textMsg.SetText(text, color);

            Destroy(msg, 4);

        }

    }
}
