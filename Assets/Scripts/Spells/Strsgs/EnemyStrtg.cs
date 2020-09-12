using Assets.Scripts.Character;
using Assets.Scripts.Common;

namespace Assets.Scripts.Spells.Strsgs {

    public class EnemyStrtg : SpellStrategy {

        public override CharacterCtrl GetTarget(Spell spell, CharacterCtrl from) {

            var enemyTeam = GameMng.current.fightMng.GetEnemyTeamFor(from);
            return CharacterCtrlHelper.GetClosestCtrl(from, enemyTeam.aliveChars);

        }

    }
}
