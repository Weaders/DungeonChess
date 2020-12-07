using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class Heal : Spell {

        public int healAmount = 10;

        public float apPlusScale = 2f;

        public int stacks = 3;

        public GameObject healEffect;

        public float effectTime = 2f;

        [Placecholder("heal_amount")]
        public int GetHealAmount(CharacterData data) => healAmount;

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseOpts opts) {

            TagLogger<Heal>.Info("Use Heal");

            to.characterData.actions.GetHeal(from, new ActionsData.Heal(GetHealAmount(from.characterData)));

            var effect = to.effectsPlacer.PlaceEffect(healEffect);

            Destroy(effect, effectTime);

            return null;

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }
    }

}
