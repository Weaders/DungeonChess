using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Common;

namespace Assets.Scripts.Spells.Strsgs {

    public class EnemyStrtg : SpellStrategy {

        public override CharacterCtrl GetTarget(Spell spell, ISpellUse from, IEnumerable<CharacterCtrl> ctrls = null) {
            return CharacterCtrlHelper.GetClosestCtrl(from.characterCtrl, ctrls ?? GetAliveCtrls(from));
        }

    }

}
