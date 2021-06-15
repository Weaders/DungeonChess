using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;

namespace Assets.Scripts.Spells.Strsgs {

    public class RandomEnemyStrtg : SpellStrategy {

        public override CharacterCtrl GetTarget(Spell spell, ISpellUse from, IEnumerable<CharacterCtrl> ctrls = null) {

            //ctrls ??= GetAliveCtrls(from); =((( old version for unity
            if (ctrls == null)
                ctrls = GetAliveCtrls(from);

            return ctrls.ToArray()[UnityEngine.Random.Range(0, ctrls.Count())];

        }

    }

}
