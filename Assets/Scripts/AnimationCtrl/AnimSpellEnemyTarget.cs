using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Effects;
using Assets.Scripts.Spells;
using UnityEngine;
using static Assets.Scripts.AnimationCtrl.AnimEventForward;
using static Assets.Scripts.Spells.Spell;

namespace Assets.Scripts.AnimationCtrl {

    public class AnimSpellEnemyTarget : SpellAnimationData, ITargetForAnimEvents {

        public EffectObj animObjPrefab;

        public Dictionary<AnimEventForward, AnimEventData> anims = new Dictionary<AnimEventForward, AnimEventData>();

        public bool afterCharSpell;

        public override AnimRunResult RunFor(Spell spell, CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts = null) {

            var isEnded = false;

            var result = new AnimRunResult {
                animEventData = new AnimEventData(),
                IsEndAnim = () => isEnded
            };

            result.animEventData.AddListener((d) => {
                if (d.animEventType == AnimEventType.EndSpellEvent)
                    isEnded = true;
            });

            if (afterCharSpell) {
                StartCoroutine(WaitForEndAndRunSpell(opts.animator, to, result.animEventData));
            } else {
                RunEffectSpell(to, result.animEventData);
            }

            return result;

        }

        public void TriggerEvent(AnimData animData) {

            if (anims.TryGetValue(animData.animEventForward, out var val)) {
                val.Invoke(animData);
            }

        }

        private IEnumerator WaitForEndAndRunSpell(Animator animator, CharacterCtrl to, AnimEventData eventData) {

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            RunEffectSpell(to, eventData);

        }

        private IEnumerator WaitForEndAndDestory(Animator animator, AnimEventData eventData) {

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

            eventData.Invoke(new AnimData 
            {
                animEventType = AnimEventType.EndSpellEvent
            });

            Destroy(animator.gameObject);

        }

        private AnimEventData RunEffectSpell(CharacterCtrl to, AnimEventData animEventData) {

            var animObj = to.effectsPlacer.PlaceEffect(animObjPrefab.gameObject);

            var animator = animObj.GetComponent<Animator>();

            animator.enabled = false;

            var eventForward = animObj.gameObject.AddComponent<AnimEventForward>();

            eventForward.targetForAnimEvents = this;

            anims.Add(eventForward, animEventData);

            animator.enabled = true;

            StartCoroutine(WaitForEndAndDestory(animator, animEventData));

            return animEventData;

        }

    }
}
