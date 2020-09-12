using System.Collections;
using System.Runtime.CompilerServices;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using Assets.Scripts.Spells;
using UnityEngine;

namespace Assets.Scripts.Character {

    [RequireComponent(typeof(CharacterMoveCtrl), typeof(EffectsPlacer))]
    public class CharacterCtrl : MonoBehaviour {

        public CharacterData characterData;

        public CharacterMoveCtrl moveCtrl;

        public Canvas characterCanvas;

        [SerializeField]
        private Animator animator;

        public Animator anim => animator;

        private CharacterCtrl targetForAttack;

        public SliderStatCtrl hpBar;

        public SliderStatCtrl manaBar;

        public EffectsPlacer effectsPlacer;

        public void GoAttack() {

            var spell = characterData.spellsContainer.GetBaseAttackSpell();

            var strtg = SpellStrategyStorage.GetSpellStrtg(spell);

            targetForAttack = strtg.GetTarget(spell, this);

            if (targetForAttack != null) {

                var moveAction = moveCtrl.MoveTo(targetForAttack.transform, spell.range);

                moveAction.onCome.AddListener(() => StartCoroutine(AttackWhileInRange(targetForAttack)));

                moveAction.Start();

            }

        }

        private IEnumerator AttackWhileInRange(CharacterCtrl target) {

            var spell = characterData.spellsContainer.GetBaseAttackSpell();

            animator.SetBool(AnimationValStore.IS_ATTACK, true);

            while (spell.IsInRange(this, target) && !target.characterData.stats.isDie) {

                if (characterData.stats.mana.val >= characterData.stats.maxMana.val) {

                    var fullManaSpell = characterData.spellsContainer.GetFullManaSpellAttack();

                    var strtg = SpellStrategyStorage.GetSpellStrtg(fullManaSpell);
                    var targetForManaSpell = strtg.GetTarget(fullManaSpell, this);

                    if (targetForManaSpell != null) {
                        fullManaSpell.Use(this, targetForManaSpell, null);
                    }

                    characterData.stats.mana.val = 0;

                }

                yield return new WaitForFixedUpdate();
            }

            animator.SetBool(AnimationValStore.IS_ATTACK, false);

            GoAttack();

        }

        public void MakeBaseAttack(CharacterCtrl target, float scale = 1f) {

            var spell = characterData.spellsContainer.GetBaseAttackSpell();
            spell.Use(this, target, new Spell.UseOpts { scale = scale });

            TagLogger<CharacterCtrl>.Info($"{name} make a base attack for {target.name}|scale: {scale}");

        }

        public void Init() {
           
            var colorStore = StaticData.current.colorStore;

            characterData.actions.onPostGetDmg.AddSubscription(Observable.OrderVal.UIUpdate, (dmgEventData) => {
                GameMng.current.fightTextMng.DisplayText(this, dmgEventData.dmg.GetCalculateVal().ToString(), colorStore.getDmgText);
            });

            characterData.actions.onPostGetHeal.AddSubscription(Observable.OrderVal.UIUpdate, (healEventData) => {
                GameMng.current.fightTextMng.DisplayText(this, healEventData.heal.GetCalculateVal().ToString(), colorStore.getHealText);
            });

            animator.SetFloat(AnimationValStore.SPEED_ATTACK, characterData.stats.AS);

            characterData.stats.AS.onPostChange.AddSubscription(Observable.OrderVal.CharacterCtrl, (data) => {
                animator.SetFloat(AnimationValStore.SPEED_ATTACK, data.newVal);
            });

            characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.CharacterCtrl, (data) => {

                TagLogger<CharacterCtrl>.Info($"Character is die");
                animator.SetBool(AnimationValStore.IS_DEATH, data.newVal);

            });

            hpBar.SetValForObserve(characterData.stats.hp, characterData.stats.maxHp);
            manaBar.SetValForObserve(characterData.stats.mana, characterData.stats.maxMana);

            characterData.Init();

        }

        /// <summary>
        /// Called by animation
        /// </summary>
        /// <param name="scale"></param>
        public void AmimEventMakeAttack(float scale) {

            if (targetForAttack != null)
                MakeBaseAttack(targetForAttack, scale);

        }

        public Animator GetAnimator() => animator;

    }

}


