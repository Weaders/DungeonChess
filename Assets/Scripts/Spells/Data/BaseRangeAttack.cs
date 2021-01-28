using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;

namespace Assets.Scripts.Spells.Data {

    public class BaseRangeAttack : Spell, IDmgSource {

        public int dmgAmount;

        public int adScale;

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            to.characterData.actions.GetDmg(from, new Dmg(dmgAmount + from.characterData.stats.AD * adScale, this));

            return null;

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

    }
}
