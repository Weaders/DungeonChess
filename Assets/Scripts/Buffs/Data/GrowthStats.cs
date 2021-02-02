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

        [Placeholder("ad_up")]
        public int GetAdUp(CharacterData from)
            => statFieldsPerGrowth.First(s => s.stat == Stat.Ad).intVal * countForGrowth;

        [Placeholder("hp_up")]
        public int GetHpUp(CharacterData from)
            => statFieldsPerGrowth.First(s => s.stat == Stat.Hp).intVal * countForGrowth;

        public void IncrementGrowth() {

            countForGrowth++;

            foreach (var stat in statFieldsPerGrowth) {
                characterCtrl.characterData.stats.Mofify(stat, Observable.ModifyType.Plus);
            }

        }

        protected override void Apply() {

            foreach (var stat in statFieldsPerGrowth) {

                for (var i = 0; i < countForGrowth; i++) {
                    characterCtrl.characterData.stats.Mofify(stat, Observable.ModifyType.Plus);
                }

            }

        }

        protected override void DeApply() {

            foreach (var stat in statFieldsPerGrowth) {

                for (var i = 0; i < countForGrowth; i++) {
                    characterCtrl.characterData.stats.Mofify(stat, Observable.ModifyType.Minus);
                }

            }

        }

        protected override Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var placeholders = new Placeholder[statFieldsPerGrowth.Length];

            for (var i = 0; i < statFieldsPerGrowth.Length; i++) {
                placeholders[i] = new Placeholder(statFieldsPerGrowth[i].statType.ToString() + "_prop", statFieldsPerGrowth[i].observableVal.ToString());
            }

            return base.GetPlaceholders(descriptionFor).Concat(placeholders).ToArray();

        }

    }
}
