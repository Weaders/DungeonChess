using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class JumpAttack : Spell, IDmgSource {

        [SerializeField]
        private float adScale = 1f;

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            var cellForMove = GameMng.current.cellsGridMng
                .GetNeighbours(to.characterData.cell)
                .OrderBy(c => Vector2Int.Distance(c.dataPosition, from.characterData.cell.dataPosition))
                .First();

            cellForMove.StayCtrl(from);

            from.characterData.actions.MakeAttack(
                to,
                new Dmg(Mathf.RoundToInt(from.characterData.stats.AD * adScale), this)
            );

            from.characterData.onPostMakeAttack.Invoke();

            return new UseSpellResult { };

        }
    }
}
