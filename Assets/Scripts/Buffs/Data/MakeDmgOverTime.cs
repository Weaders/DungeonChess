﻿using System.Collections;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs.Data {

    public class MakeDmgOverTime : Buff, IDmgSource {

        [Placeholder("dmg")]
        public int dmg;

        [Placeholder("seconds")]
        public int time;

        public int countStacks;

        private int currentCountOfStacks = 0;

        private Coroutine currentCoroutine;

        private bool coroutineStopped = true;

        protected override void Apply() {
            currentCoroutine = StartCoroutine(MakeDmg());
        }

        private IEnumerator MakeDmg() {

            coroutineStopped = false;

            while (currentCountOfStacks < countStacks) {

                if (characterCtrl.characterData.stats.isDie)
                    break;

                characterCtrl.characterData.actions.GetDmg(fromCharacterCtrl, new Dmg(dmg, this));
                yield return new WaitForSeconds(time / countStacks);
                currentCountOfStacks++;

            }

            coroutineStopped = true;

            characterCtrl.characterData.buffsContainer.Remove(this);

        }

        protected override void DeApply() {

            if (currentCoroutine != null && !coroutineStopped)
                StopCoroutine(currentCoroutine);

        }
    }
}
