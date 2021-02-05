using Assets.Scripts.Effects;
using Assets.Scripts.StatsData;
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
    }
}
