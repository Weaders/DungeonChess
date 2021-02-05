using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class Heal : Spell {

        public int healAmount = 10;

        public float apPlusScale = 2f;

        public int stacks = 3;

        [Placeholder("heal_amount")]
        public int GetHealAmount(CharacterData data) => Mathf.RoundToInt(healAmount + .1f * data.stats.maxHp.val);

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            to.characterData.actions.GetHeal(from, new ActionsData.Heal(GetHealAmount(from.characterData)));
            return new UseSpellResult { };

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

    }

}
