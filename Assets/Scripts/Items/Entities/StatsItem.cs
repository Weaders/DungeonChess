using Assets.Scripts.Character;
using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public class StatsItem : ItemData {

        [SerializeField]
        private StatField[] statsModify;

        public override void DeEquip(CharacterData characterData) {

            foreach (var statModify in statsModify)
                characterData.stats.Mofify(statModify, true);

        }

        public override void Equip(CharacterData characterData) {

            foreach (var statModify in statsModify)
                characterData.stats.Mofify(statModify);

        }
    }
}
