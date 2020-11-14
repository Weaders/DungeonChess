using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Spells;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.AnimationCtrl.AnimEventForward;

namespace Assets.Scripts.AnimationCtrl {

    public class AnimSpellEnemyTarget : SpellAnimationData, ITargetForAnimEvents {

        public GameObject animObjPrefab;

        public Dictionary<AnimEventForward, AnimEventData> anims = new Dictionary<AnimEventForward, AnimEventData>();

        public override AnimEventData RunFor(Spell spell, CharacterCtrl from, CharacterCtrl to) {

            var animObj = Instantiate(animObjPrefab, to.transform, false);

            var eventForward = animObj.AddComponent<AnimEventForward>();

            eventForward.targetForAnimEvents = this;

            var animEventData = new AnimEventData();

            anims.Add(eventForward, animEventData);

            StartCoroutine(WaitForEnd(animObj.GetComponent<Animator>()));

            return animEventData;

        }

        public void TriggerEvent(AnimData animData) {

            if (anims.TryGetValue(animData.animEventForward, out var val)) {
                val.Invoke(animData);
            }

        }

        private IEnumerator WaitForEnd(Animator animator) {

            //var state = animator.GetCurrentAnimatorStateInfo(0);

            //while (state.normalizedTime < 1f) {
            //    Debug.Log($"{animator.GetCurrentAnimatorStateInfo(0).normalizedTime}||{state.normalizedTime}");
            //    yield return new WaitForFixedUpdate();
            //}

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            Destroy(animator.gameObject);

        }

    }
}
