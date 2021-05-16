using System.Collections.Generic;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Spells {

    public interface ISpellUse {

        Transform transform { get; }
        CharacterData characterData { get; }
        CharacterCtrl characterCtrl { get; }

    }

    public abstract class SpellStrategy {

        public abstract CharacterCtrl GetTarget(Spell spell, ISpellUse from, IEnumerable<CharacterCtrl> ctrls = null);

        protected IEnumerable<CharacterCtrl> GetAliveCtrls(ISpellUse from) 
            => GameMng.current.fightMng.GetEnemyTeamFor(from.characterData).aliveChars;

    }

}
