using Assets.Scripts.Character;

namespace Assets.Scripts.Spells.Strsgs {
    public class SelfStrtg : SpellStrategy {
        public override CharacterCtrl GetTarget(Spell spell, CharacterCtrl from) {
            return from;
        }

    }
}
