using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using UnityEngine;
using static Assets.Scripts.FightText.FightTextMsg;

namespace Assets.Scripts.FightText {

    public class FightTextMng : MonoBehaviour {

        static int i = 0;

        [SerializeField]
        private GameObject msgPrefab;

        public Vector3 startPosition;

        private Dictionary<CharacterCtrl, Queue<TextData>> ctrlsQueueMsgs = new Dictionary<CharacterCtrl, Queue<TextData>>();

        [ContextMenu("Display fight text")]
        public void DisplayTextForFirst() {

            var ctrl = FindObjectOfType<CharacterCtrl>();
            DisplayText(ctrl, "test", new SetTextOpts() { color = Color.red, icon = GameMng.current.gameData.playerManaIcon });

        }

        public void DisplayText(CharacterCtrl ctrl, string text, SetTextOpts textOpts) {

            Queue<TextData> q;

            if (!ctrlsQueueMsgs.TryGetValue(ctrl, out q)) {
                
                q = new Queue<TextData>();
                ctrlsQueueMsgs.Add(ctrl, q);

            }

            var data = new TextData(text, textOpts);

            q.Enqueue(data);

            if (q.Count == 1)
                StartCoroutine(DisplayNext(0f, ctrl, q));

        }

        private void DisplayText(TextData data, CharacterCtrl ctrl, Queue<TextData> q) {

            if (ctrl.characterCanvas == null || !ctrl.characterCanvas.isActiveAndEnabled)
                return;

            var obj = new GameObject($"ContainerForMsg_{++i}", typeof(RectTransform));

            TagLogger<FightTextMng>.Info($"Create container for msg - {i}");

            var rectTransform = obj.transform as RectTransform;

            rectTransform.SetParent(ctrl.characterCanvas.transform);
            rectTransform.localPosition = startPosition;
            rectTransform.sizeDelta = new Vector2(4, 4);

            var msg = Instantiate(msgPrefab, obj.transform);

            var textMsg = msg.GetComponent<FightTextMsg>();
            var anim = msg.GetComponent<Animator>();

            textMsg.SetText(data.text, data.opts);

            StartCoroutine(WaitAndDestroy(anim, obj, data, ctrl));            

        }

        private IEnumerator DisplayNext(float wait, CharacterCtrl ctrl, Queue<TextData> q) {

            yield return new WaitForSeconds(wait);

            while (q.Count > 0) {
                
                DisplayText(q.Dequeue(), ctrl, q);
                yield return new WaitForSeconds(.5f);

            }                

        }

        private IEnumerator WaitAndDestroy(Animator anim, GameObject obj, TextData textData, CharacterCtrl ctrl) {

            yield return new WaitUntil(() => anim != null && anim.isActiveAndEnabled && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

            TagLogger<FightTextMng>.Info($"Destroy fight text mng");

            if (obj != null) {

                if (ctrlsQueueMsgs.TryGetValue(ctrl, out var q)) {

                    if (q.Count == 0)
                        ctrlsQueueMsgs.Remove(ctrl);

                }

                Destroy(obj);
            }
                

        }

        private class TextData {

            public string text;
            public SetTextOpts opts;

            public TextData(string t, SetTextOpts o) {
                text = t; 
                opts = o;
            }

        }

    }
}
