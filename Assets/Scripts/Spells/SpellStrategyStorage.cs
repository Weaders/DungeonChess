using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Spells.Strsgs;

namespace Assets.Scripts.Spells {
    public static class SpellStrategyStorage {

        public static SpellStrategy GetSpellStrtg(Spell spell) {

            switch (spell.spellTarget) {

                case SpellTarget.Enemy:
                    return new EnemyStrtg();
                case SpellTarget.Self:
                    return new SelfStrtg();

            }

            throw new Exception($"Spell has wrong {nameof(spell.spellTarget)}");

        }


    }
}
