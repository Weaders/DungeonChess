using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Effects;
using Assets.Scripts.Spells;
using UnityEngine;
using static Assets.Scripts.AnimationCtrl.AnimEventForward;

namespace Assets.Scripts.AnimationCtrl {

    public class AnimSpellEnemyTarget : SpellAnimationData, ITargetForAnimEvents {

        public EffectObj animObjPrefab;

        public Dictionary<AnimEventForward, AnimEventData> anims = new Dictionary<AnimEventForward, AnimEventData>();

        public override AnimEventData RunFor(Spell spell, CharacterCtrl from, CharacterCtrl to) {

            var animObj = to.effectsPlacer.PlaceEffect(animObjPrefab.gameObject);

            var eventForward = animObj.gameObject.AddComponent<AnimEventForward>();

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

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            Destroy(animator.gameObject);

        }

    }
}
