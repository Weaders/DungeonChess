using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;

namespace Assets.Scripts.Fight.PlaceStrategy {

    public class PreferStrategy : TeamPlaceStrategy {

        public override void Place(FightTeam team, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells)
            => Place(team.aliveChars, cells, enemyCells);

        public override void Place(IEnumerable<CharacterCtrl> ctrls, IEnumerable<Cell> cells, IEnumerable<Cell> enemyCells) {

            var cellsForPlace = cells.Where(c => c.enemyPrefer != null && c.enemyPrefer.Length > 0);

            foreach (var cellForPlace in cellsForPlace) {

                foreach (var prefer in cellForPlace.enemyPrefer) {

                    switch (prefer) {
                        case EnemyPrefer.WithJump:

                            var ctrl = ctrls.FirstOrDefault(c => {

                                var spell = c.characterData.spellsContainer.GetOnStartSpell();
                                return spell != null && spell.ContainsFeature(Spells.Features.Jump);

                            });

                            if (ctrl != null) {

                                cellForPlace.StayCtrl(ctrl);
                                ctrls = ctrls.Except(new[] { ctrl });

                            }


                            break;

                    }

                }

            }

        }
    }
}
