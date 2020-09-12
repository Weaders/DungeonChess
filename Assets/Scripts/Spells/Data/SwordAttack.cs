using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class SwordAttack : Spell {

        private float adScale = 1f;

        [Placecholder("dmg_amount")]
        public int GetDmg(CharacterData from)
            => Mathf.RoundToInt(adScale * from.stats.AD);

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) => new[] {
            data.stats.AD
        };

        public override void Use(CharacterCtrl from, CharacterCtrl to, UseOpts useOpts) {

            to.characterData.actions.GetDmg(
                from,
                new ActionsData.Dmg(GetDmg(from.characterData))
            );

            from.characterData.onPostMakeAttack.Invoke();



        }

    }

}
