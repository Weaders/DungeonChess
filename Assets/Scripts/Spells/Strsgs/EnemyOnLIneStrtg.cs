using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Character;
using Assets.Scripts.Common;

namespace Assets.Scripts.Spells.Strsgs {
    public class EnemyOnLIneStrtg : SpellStrategy {

        public override CharacterCtrl GetTarget(Spell spell, ISpellUse from, IEnumerable<CharacterCtrl> ctrls = null) {

            ctrls = ctrls ?? GetAliveCtrls(from);

            var enemies = ctrls.Where(ctrl => ctrl.characterData.cell.dataPosition.x == from.characterData.cell.dataPosition.x);
            return CharacterCtrlHelper.GetClosestCtrl(from.characterCtrl, enemies);

        }

    }
}
