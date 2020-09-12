using Assets.Scripts.Spells.Modifiers;
using UnityEngine;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Buffs.Data {

    public class LessDmgBuff : Buff {

        public GameObject effectPrefab;

        public float dmgScale;

        private GameObject currentEffect;

        protected override void Apply() {

            if (effectPrefab != null) {
                currentEffect = characterCtrl.effectsPlacer.PlaceEffect(effectPrefab);
            }

            characterCtrl.characterData
                .actions.onPreGetDmg
                .AddSubscription(Observable.OrderVal.Buff, OnPreMakeDmg);

        }

        private void OnPreMakeDmg(DmgEventData eventData) {
            eventData.dmg.dmgModifiers.Add(new DmgScale(dmgScale, int.MaxValue));
        }

        protected override void DeApply() {

            characterCtrl.characterData
                .actions.onPreGetDmg
                .RemoveSubscription(OnPreMakeDmg);

            if (currentEffect != null) {
                Destroy(currentEffect);
            }

        }
    }
}
