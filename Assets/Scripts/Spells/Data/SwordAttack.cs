using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells.Modifiers;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class SwordAttack : Spell, IDmgSource {

        private float adScale = 1f;

        [Placeholder("dmg_amount")]
        public int GetDmg(CharacterData from)
            => Mathf.RoundToInt(adScale * from.stats.AD);

        [SerializeField]
        private Buff debuffOnAttackPrefab;

        [SerializeField]
        private bool isCrit = false;

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) => new[] {
            data.stats.AD
        };

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts useOpts) {

            var dmg = new Dmg(GetDmg(from.characterData), this);

            if (isCrit)
                dmg.dmgModifiers.Add(new CritModify(0, from.characterData.stats.critDmg));

            to.characterData.actions.GetDmg(
                from,
                dmg
            );

            if (debuffOnAttackPrefab != null)
                to.characterData.buffsContainer.AddPrefab(debuffOnAttackPrefab, from);

            from.characterData.onPostMakeAttack.Invoke();

            return null;

        }

    }

}
