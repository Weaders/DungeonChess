using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Effects;
using Assets.Scripts.StatsData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs.Data {

    public class StatsBuff : Buff {

        public StatField[] statsModify;

        public Effect effectObjPrefab;

        private StatChange[][] statChanges;

        private GameObject currentEffectObj;

        protected override void Apply() {

            statChanges = statsModify.Select(s => characterCtrl.characterData.stats.AddChange(s, this)).ToArray();

            characterCtrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Buff, DestroyEffect);

        }

        protected override void DeApply() {

            for (var i = 0; i < statChanges.Length; i++) {

                foreach (var change in statChanges[i])
                    characterCtrl.characterData.stats.RemoveChange(statsModify[i], change);

            }

            characterCtrl.characterData.stats.isDie.onPostChange.RemoveSubscription(DestroyEffect);

            if (currentEffectObj != null)
                Destroy(currentEffectObj);

        }

        protected override Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var placeholders = new Placeholder[statsModify.Length];

            for (var i = 0; i < statsModify.Length; i++) {
                placeholders[i] = new Placeholder(statsModify[i].changeStatType.ToString() + "_prop", statsModify[i].observableVal.ToString());
            }
            
            return base.GetPlaceholders(descriptionFor).Concat(placeholders).ToArray();

        }

        private void DestroyEffect() {
            if (currentEffectObj != null)
                Destroy(currentEffectObj);
        }

        private void OnDestroy() {

            DestroyEffect();

        }
    }
}
