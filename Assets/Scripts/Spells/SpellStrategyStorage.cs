﻿using System;
using Assets.Scripts.Spells.Strsgs;

namespace Assets.Scripts.Spells {
    public static class SpellStrategyStorage {

        public static SpellStrategy GetSpellStrtg(Spell spell) {

            switch (spell.spellTarget) {

                case SpellTarget.Enemy:
                    return new EnemyStrtg();
                case SpellTarget.Self:
                    return new SelfStrtg();
                case SpellTarget.RandomEnemy:
                    return new RandomEnemyStrtg();
                case SpellTarget.EnemyAOE:
                    return new EnemyStrtg();
                case SpellTarget.EnemyOnLine:
                    return new EnemyOnLIneStrtg();

            }

            throw new Exception($"Spell has wrong {nameof(spell.spellTarget)}");

        }

    }
}
