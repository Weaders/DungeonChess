using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts.Synergy {

    [CreateAssetMenu(menuName = "Synergy/ClassTypes")]
    public class SynergyDataClassTypes : SynergyData {

        [SerializeField]
        private ClassTypeData[] classTypeDatas;

        public CharacterClassType characterClassType;

        public override RecalcResult[] Recalc(IEnumerable<CharacterCtrl> ctrls) {

            var selectedCtrls = ctrls
                .Where(ctrl => ctrl.characterData.stats.classTypes.val.Contains(characterClassType))
                .ToList();

            var count = selectedCtrls.Count();            

            var enumerator = classTypeDatas.GetEnumerator();

            ClassTypeData current = null;

            while (enumerator.MoveNext() && (enumerator.Current as ClassTypeData).count <= count) {
                current = enumerator.Current as ClassTypeData;
            }

            if (current != null) {

                return new[] {

                        new RecalcResult {
                            buffResult = current.buffPrefab,
                            ctrls = selectedCtrls,
                            synergyData = this
                        }

                    };

            }

            return null;

        }

        public override Color GetLineColor() => StaticData.current.colorStore.GetLineColor(characterClassType);

        [Serializable]
        private class ClassTypeData {

            public int count;
            public Buff buffPrefab;

        }
    }
}
