using Assets.Scripts.StatsData;
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
    }
}
