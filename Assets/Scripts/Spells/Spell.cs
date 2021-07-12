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
        FullManaAttack,
        OnStart
    }

    public enum SpellTarget {
        Self, Enemy, RandomEnemy, EnemyAOE, EnemyOnLine
    }

    public enum Features {
        Jump
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

        public Features[] features;

        [SerializeField]
        private Effect effectPrefab;

        /// <summary>
        /// Use effect on "from" character before use ult
        /// </summary>
        [SerializeField]
        private Effect perAuraFromEffectPrefab;

        /// <summary>
        /// Trigger touch every seconds
        /// </summary>
        public float triggerTouchEvery = 1000;

        /// <summary>
        /// Pre Aura Effect From
        /// </summary>
        private Effect preAuraFromEffect = null;


        private List<CharacterCtrl> touchedCtrls = new List<CharacterCtrl>();

        public string GetDescription(CharacterData owner)
            => TranslateReader.GetTranslate(descriptionKey, GetPlaceholders(owner));

        public string GetTitle(CharacterData owner)
            => TranslateReader.GetTranslate(titleKey, GetPlaceholders(owner));

        public bool ContainsFeature(Features feature)
            => features != null && features.Contains(feature);

        public virtual void PreStartAttack(CharacterCtrl from) {

            if (perAuraFromEffectPrefab != null) {

                var preAura = Instantiate(perAuraFromEffectPrefab, transform);

                preAura.PlaceForCharacter(from);

                if (spellType == SpellType.FullManaAttack) {

                    void Remove() {
                        Destroy(preAura.gameObject);
                        from.characterData.onPreUseUlt.RemoveSubscription(Remove);
                    }

                    from.characterData.onPreUseUlt.AddSubscription(OrderVal.CharacterCtrl, Remove);

                }

                preAuraFromEffect = preAura;

            }
        }

        public virtual UseSpellResult UseWithEffect(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            if (preAuraFromEffect != null)
                Destroy(preAuraFromEffect.gameObject);

            if (effectPrefab != null && !to.characterData.stats.isDie) {

                var result = new UseSpellResult();

                RunEffectResult runEffectResult = null;

                runEffectResult = RunEffect(from, to, (ctrl) => {

                    if (!touchedCtrls.Contains(ctrl) && from.teamSide != ctrl.teamSide) {

                        Use(from, ctrl, opts);
                        touchedCtrls.Add(ctrl);
                        StartCoroutine(RemoveCtrlTouch(ctrl, triggerTouchEvery));

                    }

                }, (effectObj) => {

                    if (opts.isEndAttack) {
                        from.characterData.onPostMakeAttack.Invoke();
                    }

                    opts.onEnd.Invoke();

                }, opts);

                return result;

            } else {

                var useEffect = Use(from, to, opts);

                opts.onEnd.Invoke();

                return useEffect;

            }

        }

        private IEnumerator RemoveCtrlTouch(CharacterCtrl ctrl, float time) {

            yield return new WaitForSeconds(time);
            touchedCtrls.Remove(ctrl);

        }

        public abstract UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts);

        protected RunEffectResult RunEffect(CharacterCtrl from, CharacterCtrl target, UnityAction<CharacterCtrl> onTouch, UnityAction<Effect> onCome, UseSpellOpts opts) {

            Effect effectObj = Instantiate(effectPrefab, transform);

            effectObj.transform.position = from.transform.position;

            effectObj.MoveToCharacter(target, BindTarget.Head);

            if (onTouch != null)
                effectObj.moveResult.onTouch.AddListener(onTouch);

            if (onCome != null)
                effectObj.moveResult.onCome.AddListener(onCome);

            return new RunEffectResult(effectObj);
        }

        public class RunEffectResult {

            public readonly Effect effectObj;

            public RunEffectResult(Effect effect) {
                effectObj = effect;
            }

        }

        public int range = 2;

        public virtual bool IsInRange(CharacterCtrl from, CharacterCtrl to) {

            if (from.characterData.cell == null || to.characterData.cell == null || range == 0)
                return false;

            return CellRangeHelper.IsInRange(from.characterData.cell.dataPosition, to.characterData.cell.dataPosition, range);

        }

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
            public UnityEvent onEnd = new UnityEvent();

            public bool onMakeAttackEffect;

        }

        public class UseSpellResult { }

    }

}
