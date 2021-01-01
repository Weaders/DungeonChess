using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class SwordAttack : Spell, IDmgSource {

        private float adScale = 1f;

        [Placecholder("dmg_amount")]
        public int GetDmg(CharacterData from)
            => Mathf.RoundToInt(adScale * from.stats.AD);

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) => new[] {
            data.stats.AD
        };

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseOpts useOpts) {

            to.characterData.actions.GetDmg(
                from,
                new Dmg(GetDmg(from.characterData), this)
            );

            from.characterData.onPostMakeAttack.Invoke();

            from.characterData.stateContainer.AddStun(2f);

            return null;

        }

    }

}
