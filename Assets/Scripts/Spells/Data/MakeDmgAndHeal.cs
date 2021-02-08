using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class MakeDmgAndHeal : Spell, IDmgSource {

        [SerializeField]
        private int baseDmg;

        [SerializeField]
        private float scaleAD;

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) => new[]
        {
            data.stats.AD
        };

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            var calcedDmg = Mathf.RoundToInt((baseDmg + scaleAD * from.characterData.stats.AD) * opts.scale);

            var result = to.characterData.actions.GetDmg(from, new Dmg(calcedDmg, this));
            from.characterData.actions.GetHeal(from, new ActionsData.Heal(result.resultVal));

            return new UseSpellResult { };

        }

    }

}
