using System.Collections;
using System.Linq;
using Assets.Scripts.Bullet;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells;
using Assets.Scripts.Spells.Modifiers;
using Assets.Scripts.Synergy;
using UnityEngine;

namespace Assets.Scripts.Character {

    [RequireComponent(typeof(CharacterMoveCtrl), typeof(EffectsPlacer))]
    public class CharacterCtrl : MonoBehaviour, IBulletSpawner, IBulletTarget {

        public OrderedEvents onDestoy = new OrderedEvents();

        private Fight.TeamSide _teamSide;

        public CharacterData characterData;

        public CharacterMoveCtrl moveCtrl;

        public Canvas characterCanvas;

        public CharacterAnimEvents characterAnimEvents;

        [SerializeField]
        private Animator animator;

        public Animator anim => animator;

        public CharacterCtrl targetForAttack { get; private set; }

        public SliderStatCtrl hpBar;

        public SliderStatCtrl manaBar;

        public EffectsPlacer effectsPlacer;

        public Fight.TeamSide teamSide {
            get => _teamSide;
            set {
                _teamSide = value;

                if (_teamSide == Fight.TeamSide.Player) {
                    colorDetect.material.color = StaticData.current.colorStore.playerTeamDetectColor;
                } else {
                    colorDetect.material.color = StaticData.current.colorStore.enemyTeamDetectColor;
                }

            }
        }

        public SynergyCharacterPointer synergyCharacterPoint;

        [SerializeField]
        public MeshRenderer colorDetect;

        [HideInInspector]
        public Cell cell;

        [SerializeField]
        private GameObject bulletSpawnObj;

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

            while (target != null && spell.IsInRange(this, target) && !target.characterData.stats.isDie) {

                transform.LookAt(target.transform);

                if (characterData.stats.mana.val >= characterData.stats.maxMana.val) {

                    animator.SetBool(AnimationValStore.IS_USING_SPELL, true);

                    var fullManaSpell = characterData.spellsContainer.GetFullManaSpellAttack();

                    var strtg = SpellStrategyStorage.GetSpellStrtg(fullManaSpell);
                    var targetForManaSpell = strtg.GetTarget(fullManaSpell, this);

                    if (targetForManaSpell != null) {
                        
                        fullManaSpell.Use(this, targetForManaSpell, null);
                        characterData.stats.mana.val = 0;

                        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

                        animator.SetBool(AnimationValStore.IS_USING_SPELL, false);

                    }

                }

                yield return new WaitForFixedUpdate();

            }

            animator.SetBool(AnimationValStore.IS_ATTACK, false);

            if (target != null) {
                GoAttack();
            }

        }

        public void MakeBaseAttack(CharacterCtrl target, float scale = 1f) {

            var spell = characterData.spellsContainer.GetBaseAttackSpell();
            spell.Use(this, target, new Spell.UseOpts { scale = scale });

            TagLogger<CharacterCtrl>.Info($"{name} make a base attack for {target.name}|scale: {scale}");

        }

        public void Init() {

            var colorStore = StaticData.current.colorStore;

            characterAnimEvents.Init(this);

            characterData.actions.onPostGetDmg.AddSubscription(OrderVal.UIUpdate, (dmgEventData) => {
                GameMng.current.fightTextMng.DisplayText(this, dmgEventData.dmg.GetCalculateVal().ToString(), colorStore.getDmgText);
            });

            characterData.actions.onPostGetHeal.AddSubscription(OrderVal.UIUpdate, (healEventData) => {
                GameMng.current.fightTextMng.DisplayText(this, healEventData.heal.GetCalculateVal().ToString(), colorStore.getHealText);
            });

            animator.SetFloat(AnimationValStore.SPEED_ATTACK, characterData.stats.AS);

            characterData.stats.AS.onPostChange.AddSubscription(OrderVal.CharacterCtrl, (data) => {
                animator.SetFloat(AnimationValStore.SPEED_ATTACK, data.newVal);
            });

            characterData.stats.isDie.onPostChange.AddSubscription(OrderVal.CharacterCtrl, (data) => {

                TagLogger<CharacterCtrl>.Info($"GON: {gameObject.name}, CN: {characterData.name} is die");
                animator.SetBool(AnimationValStore.IS_DEATH, data.newVal);
                GetComponent<Collider>().enabled = false;
                moveCtrl.GetNavMeshAgent().enabled = false;
                characterCanvas.gameObject.SetActive(false);

            });

            characterData.actions.onPreMakeAttack.AddSubscription(Observable.OrderVal.Fight, (attack) => {

                if (Random.value <= characterData.stats.critChance && !attack.dmg.dmgModifiers.Any(m => m is CritModify)) {
                    attack.dmg.dmgModifiers.Add(new CritModify(0, characterData.stats.critDmg));
                }

            });

            hpBar.SetValForObserve(characterData.stats.hp, characterData.stats.maxHp);
            manaBar.SetValForObserve(characterData.stats.mana, characterData.stats.maxMana);

            characterData.Init();

        }

        public Animator GetAnimator() => animator;

        public Transform GetTargetTransform() => bulletSpawnObj.transform;

        public Transform GetSpawnTransform() => bulletSpawnObj.transform;

        private void OnDestroy() {
            onDestoy.Invoke();
        }

    }

}
