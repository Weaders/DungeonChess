using System.Collections;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using UnityEngine;
using static Assets.Scripts.FightText.FightTextMsg;

namespace Assets.Scripts.FightText {

    public class FightTextMng : MonoBehaviour {

        static int i = 0;

        [SerializeField]
        private GameObject msgPrefab;

        [ContextMenu("Display fight text")]
        public void DisplayTextForFirst() {

            var ctrl = FindObjectOfType<CharacterCtrl>();
            DisplayText(ctrl, "test", new SetTextOpts() { color = Color.red });

        }

        public void DisplayText(CharacterCtrl ctrl, string text, SetTextOpts textOpts) {

            var obj = new GameObject($"ContainerForMsg{++i}", typeof(RectTransform));

            TagLogger<FightTextMng>.Info($"Create container for msg - {i}");

            obj.transform.SetParent(ctrl.characterCanvas.transform);

            var msg = Instantiate(msgPrefab, obj.transform);

            var textMsg = msg.GetComponent<FightTextMsg>();

            var anim = msg.GetComponent<Animator>();

            StartCoroutine(WaitAndDestroy(anim, obj));

            textMsg.SetText(text, textOpts);

            if (Random.value > .5f) {
                obj.transform.localPosition = new Vector3(-1, 0, 0);
            } else {
                obj.transform.localPosition = new Vector3(-1, 1, 0);
            }

        }

        private IEnumerator WaitAndDestroy(Animator anim, GameObject obj) {

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            TagLogger<FightTextMng>.Info($"Destroy fight text mng");
            Destroy(obj);

        }

    }
}
