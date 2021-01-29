using System;
using System.Linq;
using Assets.Scripts.StatsData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public class StatsItem : ItemData {

        [SerializeField]
        private StatField[] statsModify;

        protected override void OnDeEquip() {

            foreach (var statModify in statsModify)
                owner.stats.Mofify(statModify, Observable.ModifyType.Minus);

        }

        protected override void Equip() {

            foreach (var statModify in statsModify)
                owner.stats.Mofify(statModify);

        }

        protected override Placeholder[] GetPlaceholders(Character.CharacterData descriptionFor) {

            var placeholders = new Placeholder[statsModify.Length];

            for (var i = 0; i < statsModify.Length; i++) {
                placeholders[i] = new Placeholder(statsModify[i].statType.ToString() + "_prop", statsModify[i].observableVal.ToString());
            }

            return base.GetPlaceholders(descriptionFor).Concat(placeholders).ToArray();

        }
    }
}
