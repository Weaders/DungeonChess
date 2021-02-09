using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.ActionsData;
using Assets.Scripts.AnimationCtrl;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells;
using Assets.Scripts.Spells.Modifiers;
using Assets.Scripts.Synergy;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    [RequireComponent(typeof(CharacterMoveCtrl), typeof(EffectsPlacer))]
    public class CharacterCtrl : MonoBehaviour {

        public OrderedEvents onDestoy = new OrderedEvents();

        private bool _isSelected = false;

        public bool isSelected {
            get => _isSelected;
            set {

                _isSelected = value;
                onSelected.Invoke();

            }
        }

        public UnityEvent onSelected = new UnityEvent();

        public UnityEvent<Cell, Cell> onChangeCell = new UnityEvent<Cell, Cell>();

        private Fight.TeamSide _teamSide;

        public CharacterData characterData;

        public CharacterMoveCtrl moveCtrl;

        public Canvas characterCanvas;

        public CharacterAnimEvents characterAnimEvents;

        public Sprite imgSprite;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private AudioSource audioSource;

        public Animator anim => animator;

        public CharacterCtrl targetForAttack { get; private set; }

        public SliderStatCtrl hpBar;

        public SliderStatCtrl manaBar;

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

        private Cell _cell;

        private Cell _draggedCell;

        private Cell _startedCell;

        public Cell startCell => _startedCell;

        public Cell cell {
            get => _cell;
            set {

                var oldVal = _cell;

                _cell = value;

                onChangeCell.Invoke(oldVal, _cell);
            }
        }

        [SerializeField]
        private GameObject bulletSpawnObj;

        [SerializeField]
        private StateIconsCtrl stateIconsCtrl;

        [SerializeField]
        private GameObject characterSlider;

        [SerializeField]
        private AudioClip clipAttack;

        public bool isCanMakeFullManaAttack => characterData.stats.mana >= characterData.stats.maxMana && characterData.isCanAttack;

        private bool isStartToFight = false;

        private bool isReadyForMove = true;

        /// <summary>
        /// After call this character start move for enemy character for attack.
        /// </summary>
        public void GoAttack() {

            _startedCell = cell;
            isStartToFight = true;

        }

        /// <summary>
        /// After call character stop move or attack, and reset position on started cell.
        /// </summary>
        public void StopAttack() {
            isStartToFight = false;
        }

        private void Update() {

            characterCanvas.transform.rotation = Quaternion.identity;

            if (!isStartToFight || !isReadyForMove || characterData.stats.isDie) {
                return;

            }

            var spell = GetSpellForUse();
            var strtg = SpellStrategyStorage.GetSpellStrtg(spell);

            targetForAttack = strtg.GetTarget(spell, this);

            if (targetForAttack != null) {

                if (spell.IsInRange(this, targetForAttack)) {
                    
                    isReadyForMove = false;
                    StartCoroutine(AttackWhileInRange(targetForAttack, () => isReadyForMove = true));

                } else if (characterData.isCanMove) {

                    var path = GameMng.current.pathToCell.GetPath(cell, targetForAttack.cell, spell.range);

                    if (path != null && path.cells.Count > 1) {

                        var cells = path.GetToMovePath();
                        isReadyForMove = false;
                        StartCoroutine(moveCtrl.MoveToCell(cells.First(), () => isReadyForMove = true));

                    }

                }

            }       

        }

        private Spell GetSpellForUse() =>
            characterData.stats.mana.val < characterData.stats.maxMana.val ?
                characterData.spellsContainer.GetBaseAttackSpell() : characterData.spellsContainer.GetFullManaSpellAttack();

        private IEnumerator AttackWhileInRange(CharacterCtrl target, UnityAction onEnd) {

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

            var spell = characterData.spellsContainer.GetBaseAttackSpell();

            spell.UseWithEffect(this, target, new Spell.UseSpellOpts {
                scale = data.scale,
                isEndAttack = data.isTiggerEndAttack
            });

            TagLogger<CharacterCtrl>.Info($"{name} make a base attack for {target.name}|scale: {data.scale}");

        }

        public void MakeFullManaAttack(CharacterCtrl target, AnimAttackData data = null) {

            var spell = characterData.spellsContainer.GetFullManaSpellAttack();

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

            stateIconsCtrl.SetCharacterData(characterData);

            characterData.actions.onPostGetDmg.AddSubscription(OrderVal.UIUpdate, (dmgEventData) => {

                var isCrit = dmgEventData.dmg.dmgModifiers.Any(m => m is CritModify);

                GameMng.current.fightTextMng.DisplayText(this, dmgEventData.dmg.GetCalculateVal().ToString(), new FightText.FightTextMsg.SetTextOpts 
                {
                    color = colorStore.getDmgText,
                    size = dmgEventData.dmg.GetCalculateVal() > 50 ? 2 : 1,
                    icon = isCrit ? GameMng.current.gameData.critIcon : null
                });

            });

            characterData.actions.onPostGetHeal.AddSubscription(OrderVal.UIUpdate, (healEventData) => {

                GameMng.current.fightTextMng.DisplayText(this, healEventData.heal.GetCalculateVal().ToString(), new FightText.FightTextMsg.SetTextOpts 
                { 
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
                //characterCanvas.gameObject.SetActive(false);
                animator.applyRootMotion = false;

            });

            characterData.actions.onPreMakeAttack.AddSubscription(OrderVal.Fight, (attack) => {

                if (UnityEngine.Random.value <= characterData.stats.critChance && !attack.dmg.dmgModifiers.Any(m => m is CritModify)) {
                    attack.dmg.dmgModifiers.Add(new CritModify(0, characterData.stats.critDmg));
                }

            });

            characterData.actions.onPostGetDmg.AddSubscription(OrderVal.Fight, dmgData => {

                if (characterData.stats.vampirism > 0) {
                    
                    characterData.actions.GetHeal(this, new Heal(
                        Mathf.RoundToInt(dmgData.dmg.GetCalculateVal() * characterData.stats.vampirism.val)
                    ));

                }

            });

            characterData.buffsContainer.onAdd.AddSubscription(OrderVal.CharacterCtrl, (buff) => {

                if (!characterData.buffsContainer.isInProcessOfInit && buff.data.GetBuffType() == Buffs.BuffType.Buff)
                    effectsPlacer.PlaceEffect(GameMng.current.gameData.onGetGoodEffect.gameObject);                

            });

            characterData.actions.onPostGetHeal.AddSubscription(OrderVal.CharacterCtrl, () => {

                if (effectsPlacer != null)
                    effectsPlacer.PlaceEffect(GameMng.current.gameData.healingEffect.gameObject);

            });

            characterData.itemsContainer.onSet.AddSubscription(OrderVal.CharacterCtrl, () => {

                OnRefreshActionCells();
                OnRefreshShowCellsEffects();

            });

            onChangeCell.AddListener((oldCell, newCell) => {

                OnRefreshActionCells();
                OnRefreshShowCellsEffects();

            });

            hpBar.SetValForObserve(characterData.stats.hp, characterData.stats.maxHp);
            manaBar.SetValForObserve(characterData.stats.mana, characterData.stats.maxMana);

        }

        public Animator GetAnimator() => animator;

        public Transform GetTargetTransform() => bulletSpawnObj.transform;

        public Transform GetSpawnTransform() => bulletSpawnObj.transform;

        public void OnDraggableToCell(Cell cell) {
            
            _draggedCell = cell;
            OnRefreshShowCellsEffects();

        }

        public Cell GetCellForSelectedDisplay() {

            if (_draggedCell != null)
                return _draggedCell;
            else
                return cell;

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
