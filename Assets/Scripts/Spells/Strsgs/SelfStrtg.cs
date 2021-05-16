using System.Collections.Generic;
using Assets.Scripts.Character;

namespace Assets.Scripts.Spells.Strsgs {
    public class SelfStrtg : SpellStrategy {

        public override CharacterCtrl GetTarget(Spell spell, ISpellUse from, IEnumerable<CharacterCtrl> ctrls = null) {
            return from.characterCtrl;
        }

    }
}
