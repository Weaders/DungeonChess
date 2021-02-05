using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {
    public class MakeAttackAndGetBuff : Spell, IDmgSource {

        public bool isCrit;

        public Buff buffToGetForCharacter;

        public float timeForBuff;

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }

        public override UseSpellResult Use(CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts) {

            var dmg = new Dmg(from.characterData.stats.AD, this);

            if (from != null)
                from.characterData.actions.MakeAttack(to, dmg);
            else
                to.characterData.actions.GetDmg(from, dmg);

            if (buffToGetForCharacter != null) {
                
                var buff = from.characterData.buffsContainer.AddPrefab(buffToGetForCharacter);
                StartCoroutine(RemoveBuff(from, buff, timeForBuff));

            }

            return null;

        }

        private IEnumerator RemoveBuff(CharacterCtrl from, Buff buff, float timeWait) {

            yield return new WaitForSeconds(timeWait);

            if (from != null) {
                from.characterData.buffsContainer.Remove(buff);
            }

        }
    }
}
