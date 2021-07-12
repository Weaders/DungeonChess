using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.ActionsData;
using Assets.Scripts.AnimationCtrl;
using Assets.Scripts.Buffs;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells;
using Assets.Scripts.Spells.Modifiers;
using Assets.Scripts.Synergy;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    [RequireComponent(typeof(CharacterMoveCtrl))]
    public class CharacterCtrl : MonoBehaviour, ISpellUse, IForActions {

        public OrderedEvents onDestoy = new OrderedEvents();

        private bool _isSelected = false;

        public CharacterCtrl characterCtrl => this;

        CharacterData ISpellUse.characterData => characterData;

        CharacterMoveCtrl IForActions.characterMoveCtrl => moveCtrl;

        public bool isSelected {
            get => _isSelected;
            set {

                _isSelected = value;
                onSelected.Invoke();

            }
        }

        public UnityEvent onSelected = new UnityEvent();

        private Fight.TeamSide _teamSide;

        public CharacterData characterData;

        public CharacterMoveCtrl moveCtrl;

        public Canvas characterCanvas;

        public MessageContainer messageContainer;

        public CharacterAnimEvents characterAnimEvents;

        public Sprite imgSprite;

        public Transform headTransform;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private AudioSource audioSource;

        public Animator anim => animator;

        public CharacterCtrl targetForAttack { get; private set; }

        public ProgressBarCtrl hpBar;

        public ProgressBarCtrl manaBar;

        public EffectsPlacer effectsPlacer;

        public Fight.TeamSide teamSide {
            get => _teamSide;
            set {
                _teamSide = value;
            }
        }

        [HideInInspector]
        [Obsolete]
        public SynergyCharacterPointer synergyCharacterPoint;

        private Cell _draggedCell;

        private Cell _startedCell;

        public Cell startCell => _startedCell;


        [SerializeField]
        private GameObject bulletSpawnObj;

        [SerializeField]
        private GameObject characterSlider;

        [SerializeField]
        private AudioClip clipAttack;

        /// <summary>
        /// Effect that will be played on make base attack
        /// </summary>
        [SerializeField]
        private Effect onBaseAttakEffect;

        /// <summary>
        /// Effect, that will be executed on dead
        /// </summary>
        [SerializeField]
        private Effect onDeadEffect;

        /// <summary>
        /// Character materials
        /// </summary>
        [SerializeField]
        private Renderer[] charactersRenders;

        public bool isCanMakeFullManaAttack => characterData.stats.mana >= characterData.stats.maxMana && characterData.isCanAttack;

        public bool isCanAttack { get; set; } = false;

        public bool isReadyForMove { get; set; } = true;


        protected CharacterActionsCtrl characterActionsCtrl { get; set; }

        private void Awake() {
            
            characterActionsCtrl = new CharacterActionsCtrl(this);

            if (charactersRenders == null || charactersRenders.Length == 0)
                charactersRenders = GetComponentsInChildren<Renderer>();

        }


        public virtual bool IsTargetableFor(CharacterCtrl characterCtrl) {
            return true;
        }

        /// <summary>
        /// After call this character start move for enemy character for attack.
        /// </summary>
        public void GoAttack() {

            _startedCell = characterData.cell;
            isCanAttack = true;

        }

        /// <summary>
        /// After call character stop move or attack, and reset position on started cell.
        /// </summary>
        public void StopAttack() {
            isCanAttack = false;
        }

        public IEnumerator GetActionsToDo() {

            characterCanvas.transform.rotation = Quaternion.identity;

            if (!isCanAttack || !isReadyForMove || characterData.stats.isDie) {
                return null;
            }

            return characterActionsCtrl.IterateAction();

        }

        public IEnumerator DeadCoroutine() {

            var f = 0f;

            if (onDeadEffect != null)
                onDeadEffect.Play();

            while (f < 1f){
                
                f += Time.deltaTime;

                foreach (var render in charactersRenders)
                    render.material.SetFloat("_Dissolve", f);

                yield return null;

            }

            if (onDeadEffect != null)
                onDeadEffect.Stop();

        }

        public Spell GetSpellForUse() =>
            characterData.stats.mana.val < characterData.stats.maxMana.val ?
                characterData.spellsContainer.GetBaseAttackSpell() : characterData.spellsContainer.GetFullManaSpellAttack();

        public virtual IEnumerator AttackWhileInRange(CharacterCtrl target, UnityAction onEnd) {

            while (target != null && !target.characterData.stats.isDie && !characterData.stats.isDie) {

                var spell = GetSpellForUse();

                var strtg = SpellStrategyStorage.GetSpellStrtg(spell);
                targetForAttack = target = strtg.GetTarget(spell, this);

                if (target == null || !spell.IsInRange(this, target))
                    break;

                animator.SetBool(AnimationValStore.IS_USING_SPELL, spell.spellType == SpellType.FullManaAttack);
                animator.SetBool(AnimationValStore.IS_ATTACK, true);

                if (characterData.isCanAttack) {

                    transform.LookAt(target.transform);

                    if (spell.spellType == SpellType.FullManaAttack) {
                        animator.SetBool(AnimationValStore.IS_USING_SPELL, true);
                    }

                } else {

                    animator.SetBool(AnimationValStore.IS_USING_SPELL, false);
                    animator.SetBool(AnimationValStore.IS_ATTACK, false);

                }

                yield return new WaitForFixedUpdate();

            }

            animator.SetBool(AnimationValStore.IS_ATTACK, false);
            animator.SetBool(AnimationValStore.IS_USING_SPELL, false);

            onEnd?.Invoke();

        }

        public void MakeBaseAttack(CharacterCtrl target, AnimAttackData data = null) {

            if (clipAttack != null)
                audioSource.PlayOneShot(clipAttack);

            if (data.isWithEffect && onBaseAttakEffect != null)
                onBaseAttakEffect.Play();

            var spell = characterData.spellsContainer.GetBaseAttackSpell();

            spell.UseWithEffect(this, target, new Spell.UseSpellOpts {
                scale = data.scale,
                isEndAttack = data.isTiggerEndAttack
            });

            TagLogger<CharacterCtrl>.Info($"{name} make a base attack for {target.name}|scale: {data.scale}");

        }

        /// <summary>
        /// Called wheen start anim for attack
        /// </summary>
        public void StartMakeAttack(bool isFullMana) {

            if (isFullMana) {
                characterData.spellsContainer.GetFullManaSpellAttack().PreStartAttack(this);

            } else {
                characterData.spellsContainer.GetBaseAttackSpell().PreStartAttack(this);
            }

        }

        public void MakeFullManaAttack(CharacterCtrl target, AnimAttackData data = null) {

            var spell = characterData.spellsContainer.GetFullManaSpellAttack();

            characterData.onPreUseUlt.Invoke();

            spell.UseWithEffect(this, target, new Spell.UseSpellOpts {
                scale = data.scale,
                isEndAttack = data.isTiggerEndAttack
            });

            characterData.stats.mana.val = 0;

            TagLogger<CharacterCtrl>.Info($"{name} make a full mana attack for {target.name}| scale: {data.scale}");

            if (data.isTiggerEndAttack) {
                animator.SetBool(AnimationValStore.IS_USING_SPELL, false);
            }

        }

        public void Init() {

            characterData.Init();

            var colorStore = StaticData.current.colorStore;

            characterAnimEvents.Init(this);

            characterData.actions.onPostGetDmg.AddSubscription(OrderVal.UIUpdate, (dmgEventData) => {

                var isCrit = dmgEventData.dmg.dmgModifiers.Any(m => m is CritModify);

                var val = dmgEventData.dmg.GetCalculateVal();

                GameMng.current.fightTextMng.DisplayText(this, val.ToString(), new FightText.FightTextMsg.SetTextOpts {
                    color = val == 0 ? colorStore.noDmgText : colorStore.getDmgText,
                    size = dmgEventData.dmg.GetCalculateVal() > 50 ? 2 : 1,
                    icon = isCrit ? GameMng.current.gameData.critIcon : null
                });

            });

            characterData.actions.onPostGetHeal.AddSubscription(OrderVal.UIUpdate, (healEventData) => {

                GameMng.current.fightTextMng.DisplayText(this, healEventData.heal.GetCalculateVal().ToString(), new FightText.FightTextMsg.SetTextOpts {
                    color = colorStore.getHealText
                });

            });

            animator.SetFloat(AnimationValStore.SPEED_ATTACK, characterData.stats.AS);

            characterData.stats.AS.onPostChange.AddSubscription(OrderVal.CharacterCtrl, (data) => {
                animator.SetFloat(AnimationValStore.SPEED_ATTACK, data.newVal);
            });

            characterData.stats.isDie.onPostChange.AddSubscription(OrderVal.CharacterCtrl, (data) => {

                TagLogger<CharacterCtrl>.Info($"GON: {gameObject.name}, CN: {characterData.name} is die");
                
                animator.SetBool(AnimationValStore.IS_DEATH, data.newVal);

                GetComponent<Collider>().enabled = false;

                characterSlider.gameObject.SetActive(false);
                animator.applyRootMotion = false;

                FullTeamBuffMng.Recalc();

                StartCoroutine(DeadCoroutine());

            });

            characterData.actions.onPreMakeAttack.AddSubscription(OrderVal.Fight, attack => {

                if (UnityEngine.Random.value <= characterData.stats.critChance && !attack.dmg.dmgModifiers.Any(m => m is CritModify)) {
                    attack.dmg.dmgModifiers.Add(new CritModify(0, characterData.stats.critDmg));
                }

            });

            characterData.actions.onPostMakeDmg.AddSubscription(OrderVal.Fight, dmgData => {

                if (characterData.stats.vampirism > 0) {

                    characterData.actions.GetHeal(this, new Heal(
                        Mathf.RoundToInt(dmgData.dmg.GetCalculateVal() * characterData.stats.vampirism.val)
                    ));

                }

            });

            characterData.buffsContainer.onAdd.AddSubscription(OrderVal.CharacterCtrl, (buff) => {

                // TODO: Replace effect
                //if (!characterData.buffsContainer.isInProcessOfInit && buff.data.GetBuffType() == Buffs.BuffType.Buff)
                //    effectsPlacer.PlaceEffect(GameMng.current.gameData.onGetGoodEffect.gameObject);

            });

            characterData.actions.onPostGetHeal.AddSubscription(OrderVal.CharacterCtrl, () => {

                //TODO: Place effecct heal
                //if (effectsPlacer != null)
                //    effectsPlacer.PlaceEffect(GameMng.current.gameData.healingEffect.gameObject);

            });

            characterData.itemsContainer.onSet.AddSubscription(OrderVal.CharacterCtrl, () => {

                OnRefreshActionCells();
                OnRefreshShowCellsEffects();

            });

            characterData.onChangeCell.AddListener((oldCell, newCell) => {

                OnRefreshActionCells();
                OnRefreshShowCellsEffects();

                if (GameMng.current != null && !GameMng.current.fightMng.isInFight && newCell != null)
                    GameMng.current.simulateCharacterMoveCtrl.Simulate();



            });

            if (hpBar != null)
                hpBar.SetValForObserve(characterData.stats.hp, characterData.stats.maxHp);

            if (manaBar != null)
                manaBar.SetValForObserve(characterData.stats.mana, characterData.stats.maxMana);

            isReadyForMove = true;

        }

        public Animator GetAnimator() => animator;

        public Transform GetTargetTransform() => bulletSpawnObj.transform;

        public Transform GetSpawnTransform() => bulletSpawnObj.transform;

        public void OnDraggableToCell(Cell cell) {

            if (_draggedCell != null) {
                _draggedCell.RemoveState(Cell.CellState.NotAvailable);
            }

            _draggedCell = cell;

            if (_draggedCell != null) {
                _draggedCell.AddState(Cell.CellState.NotAvailable);
            }

            OnRefreshShowCellsEffects();

        }

        public Cell GetCellForSelectedDisplay() {

            if (_draggedCell != null)
                return _draggedCell;
            else
                return characterData.cell;

        }

        public void OnRefreshShowCellsEffects() {

            foreach (var item in characterData.itemsContainer.GetItems()) {

                if (item is IDisplayOnSelect display) {
                    display.HideFor();
                }

            }

            foreach (var item in characterData.itemsContainer.GetItems()) {

                if (item is IDisplayOnSelect display) {
                    display.ShowFor();
                }

            }

        }

        public void OnRefreshActionCells() {

            foreach (var item in characterData.itemsContainer.GetItems()) {

                if (item is IAddCellActions display) {
                    display.RemoveActionsFor();
                }

            }

            foreach (var item in characterData.itemsContainer.GetItems()) {

                if (item is IAddCellActions display) {
                    display.AddActionsFor();
                }

            }

        }

        public void OnRefreshShowCellsEffects<T>(T oldVal, T newVal)
            => OnRefreshShowCellsEffects();

        public void OnRefreshHide() {

            foreach (var item in characterData.itemsContainer.GetItems()) {

                if (item is IAddCellActions actions)
                    actions.RemoveActionsFor();

            }

        }

        private void OnDestroy() {
            onDestoy.Invoke();
        }

    }

}
