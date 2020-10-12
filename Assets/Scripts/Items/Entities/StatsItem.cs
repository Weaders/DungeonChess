using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public class StatsItem : ItemData {

        [SerializeField]
        private StatField[] statsModify;

        protected override void OnDeEquip() {

            foreach (var statModify in statsModify)
                owner.stats.Mofify(statModify, true);

        }

        protected override void Equip() {

            foreach (var statModify in statsModify)
                owner.stats.Mofify(statModify);

        }
    }
}
