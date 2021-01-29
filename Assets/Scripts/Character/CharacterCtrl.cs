using System.Collections;
using System.Linq;
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

        private Cell _cell;

        private Cell _draggedCell;

        private Cell _startedCell;

        public Cell startCell => _startedCell;

        [SerializeField]
        public MeshRenderer colorDetect;

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

            //if (!characterData.stats.isDie)
            //    _startedCell.StayCtrl(this);

        }

        private void Update() {

            characterCanvas.transform.rotation = Quaternion.identity;

            if (!isStartToFight || !isReadyForMove || characterData.stats.isDie)
                return;

            var spell = GetSpellForUse();
            var strtg = SpellStrategyStorage.GetSpellStrtg(spell);

            targetForAttack = strtg.GetTarget(spell, this);

            if (targetForAttack != null) {

                if (spell.IsInRange(this, targetForAttack)) {
                    
                    isReadyForMove = false;
                    StartCoroutine(AttackWhileInRange(targetForAttack, () => isReadyForMove = true));

                } else if (characterData.isCanMove) {

                    var path = GameMng.current.pathToCell.GetPath(cell, targetForAttack.cell);

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

            animator.SetBool(AnimationValStore.IS_USING_SPELL, GetSpellForUse().spellType == SpellType.FullManaAttack);
            animator.SetBool(AnimationValStore.IS_ATTACK, true);

            while (target != null && !target.characterData.stats.isDie) {

                var spell = GetSpellForUse();

                if (!spell.IsInRange(this, target))
                    break;

                if (characterData.isCanAttack) {

                    transform.LookAt(target.transform);

                    if (spell.spellType == SpellType.FullManaAttack) {
                        animator.SetBool(AnimationValStore.IS_USING_SPELL, true);
                    }

                }

                yield return new WaitForFixedUpdate();

            }

            animator.SetBool(AnimationValStore.IS_ATTACK, false);

            onEnd?.Invoke();

        }

        public void MakeBaseAttack(CharacterCtrl target, AnimAttackData data = null) {

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
                
                GameMng.current.fightTextMng.DisplayText(this, dmgEventData.dmg.GetCalculateVal().ToString(), new FightText.FightTextMsg.SetTextOpts 
                {
                    color = colorStore.getDmgText,
                    size = dmgEventData.dmg.GetCalculateVal() > 50 ? 2 : 1
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
                characterCanvas.gameObject.SetActive(false);

            });

            characterData.actions.onPreMakeAttack.AddSubscription(OrderVal.Fight, (attack) => {

                if (Random.value <= characterData.stats.critChance && !attack.dmg.dmgModifiers.Any(m => m is CritModify)) {
                    attack.dmg.dmgModifiers.Add(new CritModify(0, characterData.stats.critDmg));
                }

            });

            characterData.buffsContainer.onAdd.AddSubscription(OrderVal.CharacterCtrl, () => {

                if (!characterData.buffsContainer.isInProcessOfInit)
                    effectsPlacer.PlaceEffect(GameMng.current.gameData.onGetGoodEffect.gameObject);                

            });

            characterData.itemsContainer.onSet.AddSubscription(OrderVal.CharacterCtrl, () => {

                OnRefreshActionCells();
                //if (isSelected)
                OnRefreshShowCellsEffects();

            });

            onChangeCell.AddListener((oldCell, newCell) => {

                OnRefreshActionCells();

                //if (isSelected) {
                    OnRefreshShowCellsEffects();
                //}

            });

            //onSelected.AddListener(() => {

            //    if (isSelected)
            //        OnRefreshShowCellsEffects();
            //    else
            //        OnRefreshShowCellsEffects();

            //});

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
