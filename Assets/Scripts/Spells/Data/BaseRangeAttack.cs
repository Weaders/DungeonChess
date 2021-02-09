using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;

namespace Assets.Scripts.Spells.Data {

    public class BaseRangeAttack : Spell, IDmgSource {

        public int dmgAmount;

        public int adScale;

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            var dmg = new Dmg(dmgAmount + from.characterData.stats.AD * adScale, this);

            if (from != null)
                from.characterData.actions.MakeAttack(to, dmg);
            else
                to.characterData.actions.GetDmg(from, dmg);

            return null;

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

    }
}
