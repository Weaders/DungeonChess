using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.StatsData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs.Data {

    public class GrowthStats : Buff {

        [SerializeField]
        private int countForGrowth;

        public StatField[] statFieldsPerGrowth;

        public StatChange[][] changes;

        [Placeholder("ad_up")]
        public int GetAdUp(CharacterData from)
            => statFieldsPerGrowth.First(s => s.changeStatType == ChangeStatType.Ad).intVal * countForGrowth;

        [Placeholder("hp_up")]
        public int GetHpUp(CharacterData from)
            => statFieldsPerGrowth.First(s => s.changeStatType == ChangeStatType.Hp).intVal * countForGrowth;



        public void IncrementGrowth() {

            countForGrowth++;

            DeApply();

            Apply();

        }

        protected override void Apply() {

            changes = statFieldsPerGrowth
                .Select(s => characterCtrl.characterData.stats.AddChange(s, this))
                .ToArray();

        }

        protected override void DeApply() {

            for (var i = 0; i < changes.Length; i++) {

                foreach(var change in changes[i])
                    characterCtrl.characterData.stats.RemoveChange(statFieldsPerGrowth[i], change);

            }

        }

        protected override Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var placeholders = new Placeholder[statFieldsPerGrowth.Length];

            for (var i = 0; i < statFieldsPerGrowth.Length; i++) {
                placeholders[i] = new Placeholder(statFieldsPerGrowth[i].changeStatType.ToString() + "_prop", statFieldsPerGrowth[i].observableVal.ToString());
            }

            return base.GetPlaceholders(descriptionFor).Concat(placeholders).ToArray();

        }

    }
}
