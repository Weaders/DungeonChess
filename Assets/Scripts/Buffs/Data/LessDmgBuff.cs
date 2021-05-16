using Assets.Scripts.Spells.Modifiers;
using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Buffs.Data {

    public class LessDmgBuff : Buff {

        public GameObject effectPrefab;

        public float dmgScale;

        [Placeholder("seconds")]
        public float everySeconds;

        private GameObject currentEffect;

        private bool isCanUse = true;

        protected override void Apply() {

            if (effectPrefab != null) {
                currentEffect = characterCtrl.effectsPlacer.PlaceEffectWithoutTime(effectPrefab);
            }

            characterCtrl.characterData
                .actions
                .onPreGetDmg
                .AddSubscription(Observable.OrderVal.Buff, OnPreGetDmg);

        }

        private void OnPreGetDmg(DmgEventData eventData) {

            if (isCanUse) {
                eventData.dmg.dmgModifiers.Add(new DmgScale(dmgScale, int.MaxValue));
            }
            
            if (isCanUse && everySeconds > 0) {

                isCanUse = false;
                Invoke(nameof(CauUseToTrue), everySeconds);

            }

        }

        protected override void DeApply() {

            characterCtrl.characterData
                .actions.onPreGetDmg
                .RemoveSubscription(OnPreGetDmg);

            if (currentEffect != null) {
                Destroy(currentEffect);
            }

        }

        private void CauUseToTrue() {
            isCanUse = true;
        }
    }
}
