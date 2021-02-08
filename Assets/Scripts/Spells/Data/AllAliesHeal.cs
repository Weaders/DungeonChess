using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;

namespace Assets.Scripts.Spells.Data {
    public class AllAliesHeal : Spell {

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            var team = GameMng.current.fightMng.GetTeamFor(from);

            foreach (var character in team.aliveChars) {
                character.characterData.actions.GetHeal(from, new ActionsData.Heal(character.characterData.stats.maxHp));
            }

            return null;

        }
    }
}
