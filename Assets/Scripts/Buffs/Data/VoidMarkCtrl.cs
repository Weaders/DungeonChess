using Assets.Scripts.ActionsData;
using Assets.Scripts.Effects;
using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Buffs.Data {

    public class VoidMarkCtrl : Buff {

        [SerializeField]
        private float healOnGetDmg;

        [SerializeField]
        private int seconds = 10;

        [SerializeField]
        private EffectObj effectObjPrefab;

        private EffectObj effectObj;

        [Placeholder("heal_percent")]
        public int GetHealPercent()
            => Mathf.RoundToInt(healOnGetDmg * 100);

        private void OnGetDmg(DmgEventData dmgEventData) {

            var heal = Mathf.RoundToInt(dmgEventData.dmg.GetCalculateVal() * healOnGetDmg);
            dmgEventData.from.characterData.actions.GetHeal(fromCharacterCtrl, new Heal(heal));

        }

        protected override void Apply() {

            characterCtrl.characterData.actions.onPostGetDmg.AddSubscription(Observable.OrderVal.Buff, OnGetDmg);

            effectObj = Instantiate(effectObjPrefab);

            effectObj.StayOnCharacterCtrl(characterCtrl, seconds * 1000);

            Invoke("RemoveFromCurrent", seconds);

        }

        protected override void DeApply() {

            characterCtrl.characterData.actions.onPostGetDmg.RemoveSubscription(OnGetDmg);

            if (effectObj != null)
                Destroy(effectObj.gameObject);

        }
    }
}
