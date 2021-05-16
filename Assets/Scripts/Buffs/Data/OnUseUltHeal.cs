using Assets.Scripts.Character;
using Assets.Scripts.Translate;
using UnityEngine;

namespace Assets.Scripts.Buffs.Data {

    public class OnUseUltHeal : Buff {

        [Placeholder("percent")]
        public float GetPercent(CharacterData from)
            => (GetPercentVal(from.characterCtrl) * 100f);

        [Placeholder("percentUpgrade")]
        public float GetPercentUpgrade(CharacterData from)
            => percentUpgrade * 100f;

        public float GetPercentVal(CharacterCtrl from)
            => GetUpgradeLvl(from) * percentUpgrade + percentMaxHpHeal;

        public float percentMaxHpHeal = 0.05f;

        public float percentUpgrade = 0.1f;

        protected override void Apply() {
            characterCtrl.characterData.onPreUseUlt.AddSubscription(Observable.OrderVal.CharacterCtrl, OnPreUseUlt);
        }

        protected override void DeApply() {
            characterCtrl.characterData.onPreUseUlt.RemoveSubscription(OnPreUseUlt);
        }

        protected void OnPreUseUlt() {

            characterCtrl.characterData.actions.GetHeal(
                characterCtrl,
                new ActionsData.Heal(Mathf.RoundToInt(characterCtrl.characterData.stats.maxHp.val * GetPercentVal(characterCtrl)))
            );

        }

    }

}
