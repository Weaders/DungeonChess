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
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    [RequireComponent(typeof(CharacterMoveCtrl), typeof(EffectsPlacer))]
    public class CharacterCtrl : MonoBehaviour, IBulletSpawner, IBulletTarget {

        public OrderedEvents onDestoy = new OrderedEvents();

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

        private bool isShowedSelected;

        public void GoAttack() {
            StartCoroutine(GoAttackWhileNotDead());
        }

        private IEnumerator GoAttackWhileNotDead() {

            bool isStartMove = false;

            while (!isStartMove && !characterData.stats.isDie) {

                if (characterData.isCanMove) {

                    var spell = characterData.spellsContainer.GetBaseAttackSpell();

                    var strtg = SpellStrategyStorage.GetSpellStrtg(spell);

                    targetForAttack = strtg.GetTarget(spell, this);

                    if (targetForAttack != null) {

                        var moveAction = moveCtrl.MoveToCharacter(targetForAttack);

                        moveAction.onCome.AddListener(() => StartCoroutine(AttackWhileInRange(targetForAttack)));

                        isStartMove = true;
                        moveAction.Start();

                    }

                }

                yield return new WaitForFixedUpdate();

            }

        }

        private IEnumerator AttackWhileInRange(CharacterCtrl target) {

            var spell = characterData.spellsContainer.GetBaseAttackSpell();

            animator.SetBool(AnimationValStore.IS_USING_SPELL, characterData.stats.mana.val >= characterData.stats.maxMana.val);
            animator.SetBool(AnimationValStore.IS_ATTACK, true);

            while (target != null && spell.IsInRange(this, target) && !target.characterData.stats.isDie) {

                if (characterData.isCanAttack) {

                    transform.LookAt(target.transform);

                    if (characterData.stats.mana.val >= characterData.stats.maxMana.val) {

                        animator.SetBool(AnimationValStore.IS_USING_SPELL, true);

                        var fullManaSpell = characterData.spellsContainer.GetFullManaSpellAttack();

                        var strtg = SpellStrategyStorage.GetSpellStrtg(fullManaSpell);
                        var targetForManaSpell = strtg.GetTarget(fullManaSpell, this);

                        if (targetForManaSpell != null) {

                            TagLogger<CharacterCtrl>.Info($"{gameObject.name} is start using spell");

                            var useResult = fullManaSpell.Use(this, targetForManaSpell, new Spell.UseOpts {
                                animator = animator
                            });

                            characterData.stats.mana.val = 0;

                            yield return new WaitUntil(() => useResult.IsEndUseSpell());

                            TagLogger<CharacterCtrl>.Info($"{gameObject.name} is stop using spell");

                            animator.SetBool(AnimationValStore.IS_USING_SPELL, false);

                        }

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
                moveCtrl.GetNavMeshAgent().enabled = false;
                characterCanvas.gameObject.SetActive(false);

            });

            characterData.actions.onPreMakeAttack.AddSubscription(OrderVal.Fight, (attack) => {

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

        public void OnDraggableToCell(Cell cell) {
            _draggedCell = cell;
            OnRefreshShow();
        }

        public void ShowWhenSelected() {

            if (!isShowedSelected) {

                isShowedSelected = true;
                OnRefreshShow();
                characterData.itemsContainer.onSet.AddSubscription(OrderVal.CharacterCtrl, OnRefreshShow);
                onChangeCell.AddListener(OnRefreshShow);

            }
            

        }

        public void HideWhenDeselected() {

            if (isShowedSelected) {

                isShowedSelected = false;
                OnRefreshHide();
                characterData.itemsContainer.onSet.RemoveSubscription(OnRefreshShow);
                onChangeCell.RemoveListener(OnRefreshShow);

            }
        }

        public Cell GetCellForSelectedDisplay() {

            if (_draggedCell != null)
                return _draggedCell;
            else
                return cell;

        }

        public void OnRefreshShow() {


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

        public void OnRefreshShow<T>(T oldVal, T newVal)
            => OnRefreshShow();

        public void OnRefreshHide() {

            foreach (var item in characterData.itemsContainer.GetItems()) {

                if (item is IDisplayOnSelect display) {
                    display.HideFor();
                }

            }

        }

        private void Update() {
            characterCanvas.transform.rotation = Quaternion.identity;
        }

        private void OnDestroy() {
            onDestoy.Invoke();
        }

    }

}
