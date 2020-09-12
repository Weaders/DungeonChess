using Assets.Scripts.Character;
using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public class StatsItem : ItemData {

        [SerializeField]
        private Stats statsModify;

        public override void DeEquip(CharacterData characterData) {
            characterData.stats -= statsModify;
        }

        public override void Equip(CharacterData characterData) {
            characterData.stats += statsModify;
        }
    }
}
