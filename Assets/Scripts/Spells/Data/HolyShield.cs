﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Spells.Data {

    public class HolyShield : Spell {

        public Buff holyShieldBuffPrefab;

        public float timeEffect;

        [Placecholder("time_use")]
        public float GetTimeUse(CharacterData from) => timeEffect;

        public override void Use(CharacterCtrl from, CharacterCtrl to, UseOpts opts) {

            from.characterData.buffsContainer.AddPrefab(holyShieldBuffPrefab);
            StartCoroutine(RemoveEffect(from));

        }

        public IEnumerator RemoveEffect(CharacterCtrl from) {

            yield return new WaitForSeconds(timeEffect);
            from.characterData.buffsContainer.Remove(holyShieldBuffPrefab.GetId());

        }

        public override IEnumerable<ObservableVal> GetObservablesForModify(CharacterData data) {
            return new ObservableVal[] { };
        }
    }
}
