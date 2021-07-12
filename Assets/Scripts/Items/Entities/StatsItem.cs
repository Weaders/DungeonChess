using System;
using System.Linq;
using Assets.Scripts.StatsData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public class StatsItem : ItemData {

        [SerializeField]
        private StatField[] statsModify;

        private StatChange[][] statChanges;

        protected override void OnDeEquip() {

            for (var i = 0; i < statChanges.Length; i++) {

                foreach(var change in statChanges[i])
                    owner.stats.RemoveChange(statsModify[i], change);

            }

            statChanges = new StatChange[0][];

        }

        protected override void Equip() {
            statChanges = statsModify.Select(s => owner.stats.AddChange(s, this)).ToArray();
        }

        protected override Placeholder[] GetPlaceholders(Character.CharacterData descriptionFor) {

            var placeholders = new Placeholder[statsModify.Length];

            for (var i = 0; i < statsModify.Length; i++) {

                if (statsModify[i].changeStatType != ChangeStatType.None) {
                    placeholders[i] = new Placeholder(statsModify[i].changeStatType.ToString() + "_prop", statsModify[i].observableVal?.ToString());
                }

            }

            return base.GetPlaceholders(descriptionFor).Concat(placeholders).ToArray();

        }
    }
}
