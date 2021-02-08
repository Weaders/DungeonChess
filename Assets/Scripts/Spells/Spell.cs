using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Spells {

    public enum SpellType {
        BaseAttack,
        FullManaAttack
    }

    public enum SpellTarget {
        Self, Enemy, RandomEnemy, EnemyAOE
    }

    public abstract class Spell : MonoBehaviour {

        [SerializeField]
        private string id;

        public string titleKey;

        public string descriptionKey;

        public string Id => id;

        public string GetId() => Id;

        public string description
            => TranslateReader.GetTranslate(descriptionKey);

        public SpellType spellType;

        public SpellTarget spellTarget;

        public EffectObj effectObjPrefab;

        public bool instantMoveEffect = false;

        public bool waitForEndAnimation = false;

        public float effectTime = 0f;

        public float effectSpeed;

        /// <summary>
        /// Trigger touch every seconds
        /// </summary>
        public float triggerTouchEvery = 1000;

        private List<CharacterCtrl> touchedCtrls = new List<CharacterCtrl>();

        public string GetDescription(CharacterData owner)
            => TranslateReader.GetTranslate(descriptionKey, GetPlaceholders(owner));

        public string GetTitle(CharacterData owner)
            => TranslateReader.GetTranslate(titleKey, GetPlaceholders(owner));

        public virtual UseSpellResult UseWithEffect(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            if (effectObjPrefab != null && !to.characterData.stats.isDie) {

                var result = new UseSpellResult();

                RunEffectResult runEffectResult = null;

                void onDie() {

                    if (runEffectResult != null && spellTarget != SpellTarget.EnemyAOE)
                        Destroy(runEffectResult.effectObj.gameObject);

                }

                runEffectResult = RunEffect(from, to, (ctrl) => {

                    if (!touchedCtrls.Contains(ctrl)) {

                        Use(from, ctrl, opts);
                        touchedCtrls.Add(ctrl);
                        StartCoroutine(RemoveCtrlTouch(ctrl, triggerTouchEvery));

                    }

                }, (effectObj) => {

                    Destroy(effectObj.gameObject);

                    if (opts.isEndAttack) {
                        from.characterData.onPostMakeAttack.Invoke();
                    }

                    to.characterData.stats.isDie.onPostChange.RemoveSubscription(onDie);

                });

                to.characterData.stats.isDie.onPostChange.AddSubscription(OrderVal.Fight, onDie);

                return result;

            }

            return Use(from, to, opts);

        }

        private IEnumerator RemoveCtrlTouch(CharacterCtrl ctrl, float time) {

            yield return new WaitForSeconds(time);
            touchedCtrls.Remove(ctrl);

        }

        public abstract UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts);

        protected RunEffectResult RunEffect(CharacterCtrl from, CharacterCtrl target, UnityAction<CharacterCtrl> onTouch, UnityAction<EffectObj> onCome) {

            EffectObj effectObj;

            if (instantMoveEffect) {

                effectObj = Instantiate(effectObjPrefab, target.transform);
                effectObj.transform.position = target.GetSpawnTransform().position;

            } else {
                
                effectObj = Instantiate(effectObjPrefab);
                effectObj.transform.position = from.GetSpawnTransform().position;

            }

            if (spellTarget == SpellTarget.EnemyAOE) {

                effectObj.onTouch.AddListener(onTouch);
                effectObj.onCome.AddListener(() => { onCome.Invoke(effectObj); });
                effectObj.MoveToDirection(target.transform.position, effectSpeed, 2f);

            } else {

                effectObj.onCome.AddListener(() => {
                    onTouch.Invoke(target);
                    onCome.Invoke(effectObj);
                });

                if (!instantMoveEffect) {
                    effectObj.MoveToTransorm(target.GetTargetTransform(), effectSpeed);
                } else {
                    effectObj.StayOnCharacterCtrl(target, effectTime == 0f ? (float?)null : effectTime, waitForEndAnimation);
                }

            }

            return new RunEffectResult(effectObj);
        }

        public class RunEffectResult {

            public readonly EffectObj effectObj;

            public RunEffectResult(EffectObj effect) {
                effectObj = effect;
            }

        }

        public int range = 2;

        public virtual bool IsInRange(CharacterCtrl from, CharacterCtrl to)
            => CellRangeHelper.IsInRange(from.cell.dataPosition, to.cell.dataPosition, range);

        public virtual bool IsUseEffect()
            => effectObjPrefab != null;

        private Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var result = descriptionFor.stats.GetFields()
                .Select(f => new Placeholder(f.Name, f.GetValue(descriptionFor.stats).ToString()));

            result = result.Union(GetType().GetMethods().Select(p => {
                var key = p.GetCustomAttribute<PlaceholderAttribute>()?.Key;
                return key != null ? new Placeholder(key, p.Invoke(this, new[] { descriptionFor }).ToString()) : null;
            }).Where(kv => kv != null));

            return result.ToArray();

        }

        public abstract IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data);

        public class UseSpellOpts {

            public float scale = 1f;
            public bool isEndAttack = false;
            public Animator animator = null;

        }

        public class UseSpellResult {
        }

    }

}
