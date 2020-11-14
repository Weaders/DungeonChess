using System.Linq;
using Assets.Scripts.Character;

namespace Assets.Scripts.Spells.Strsgs {

    public class RandomEnemyStrtg : SpellStrategy {

        public override CharacterCtrl GetTarget(Spell spell, CharacterCtrl from) {

            var enemyTeam = GameMng.current.fightMng.GetEnemyTeamFor(from);
            var chars = enemyTeam.aliveChars.ToArray();
            return chars[UnityEngine.Random.Range(0, chars.Length)];

        }

    }

}
