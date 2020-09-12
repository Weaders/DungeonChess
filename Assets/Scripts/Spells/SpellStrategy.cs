using Assets.Scripts.Character;

namespace Assets.Scripts.Spells {
    public abstract class SpellStrategy {
        public abstract CharacterCtrl GetTarget(Spell spell, CharacterCtrl from);

    }
}
