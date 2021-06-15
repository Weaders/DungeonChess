using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Spells;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    public interface IForActions : ISpellUse {

        CharacterMoveCtrl characterMoveCtrl { get; }

        IEnumerator AttackWhileInRange(CharacterCtrl target, UnityAction onEnd);

        Spell GetSpellForUse();

        bool isReadyForMove { get; set; }
    }

    public class CharacterActionsCtrl {

        public CharacterActionsCtrl(IForActions forActions) {

            owner = forActions;

        }

        private IForActions owner;

        private CharacterCtrl targetForAttack;

        private Queue<Cell> _cellsToMove = new Queue<Cell>();

        public Path currentPath { get; private set; }

        public IEnumerable<Cell> cellsToMove => _cellsToMove;

        public bool isFirstMoveOnFight { get; set; }

        public void ClearCellToMove()
            => _cellsToMove.Clear();
        
        public IEnumerator IterateAction(bool isSimulate = false) {

            IEnumerable<CharacterCtrl> ctrls = null;

            if (isSimulate)
                ctrls = GameMng.current.simulateCharacterMoveCtrl.GetEnemyTeamFor(owner.characterCtrl).aliveChars;

            if (!isFirstMoveOnFight) {

                isFirstMoveOnFight = true;

                var startSpell = owner.characterData.spellsContainer.GetOnStartSpell();

                if (startSpell != null) {

                    var startSpellStrtg = SpellStrategyStorage.GetSpellStrtg(startSpell);

                    var targetForAttack = startSpellStrtg.GetTarget(startSpell, owner, ctrls);

                    if (targetForAttack != null && startSpell.IsInRange(owner.characterCtrl, targetForAttack)) {

                        var opts = new Spell.UseSpellOpts();

                        opts.onEnd.AddListener(() => {
                            owner.characterCtrl.isCanAttack = true;
                        });

                        owner.characterCtrl.isCanAttack = false;

                        startSpell.UseWithEffect(owner.characterCtrl, targetForAttack, opts);

                        return new object[0].GetEnumerator();

                    }

                }

            }

            var spell = owner.GetSpellForUse();
            var strtg = SpellStrategyStorage.GetSpellStrtg(spell);

            targetForAttack = strtg.GetTarget(spell, owner, ctrls);

            if (targetForAttack != null) {

                if (spell.IsInRange(owner.characterCtrl, targetForAttack)) {

                    owner.isReadyForMove = false;

                    return owner.AttackWhileInRange(targetForAttack, () => owner.isReadyForMove = true);

                } else if (owner.characterData.isCanMove) {

                    var path = GameMng.current.pathToCell.GetPath(owner.characterCtrl.characterData.cell, targetForAttack.characterData.cell, spell.range);

                    if (path != null && path.cells.Count > 1) {

                        currentPath = path;

                        var cells = path.GetToMovePath();
                        owner.isReadyForMove = false;

                        var cell = cells.First();

                        _cellsToMove.Enqueue(cell);

                        return owner.characterMoveCtrl.MoveToCell(cell, () => owner.isReadyForMove = true, isSimulate);

                    }

                }

            }

            return null;

        }

    }

}
