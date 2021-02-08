using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Effects;
using Assets.Scripts.StatsData;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs.Data {

    public class StatsBuff : Buff {

        public StatField[] statsModify;

        public EffectObj effectObjPrefab;

        private GameObject currentEffectObj;

        protected override void Apply() {

            foreach (var stat in statsModify)
                characterCtrl.characterData.stats.Mofify(stat);

            if (effectObjPrefab != null)
                currentEffectObj = characterCtrl.effectsPlacer.PlaceEffectWithoutTime(effectObjPrefab.gameObject);

        }

        protected override void DeApply() {

            foreach (var stat in statsModify)
                characterCtrl.characterData.stats.Mofify(stat, Observable.ModifyType.Minus);

            if (currentEffectObj != null)
                Destroy(currentEffectObj);

        }

        protected override Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            var placeholders = new Placeholder[statsModify.Length];

            for (var i = 0; i < statsModify.Length; i++) {
                placeholders[i] = new Placeholder(statsModify[i].statType.ToString() + "_prop", statsModify[i].observableVal.ToString());
            }
            
            return base.GetPlaceholders(descriptionFor).Concat(placeholders).ToArray();

        }
    }
}
