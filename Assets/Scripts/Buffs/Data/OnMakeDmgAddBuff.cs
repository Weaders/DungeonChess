using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Buffs.Data {
    public class OnMakeDmgAddBuff : Buff {

        public Buff buffToAddPrefab;

        protected override void Apply() {
            characterCtrl.characterData.actions.onPostMakeDmg.AddSubscription(Observable.OrderVal.Buff, OnPostMakeDmg);
        }

        private void OnPostMakeDmg(DmgEventData dmgEventData) {
            dmgEventData.to.characterData.buffsContainer.AddPrefab(buffToAddPrefab);
        }

        protected override void DeApply() {
            characterCtrl.characterData.actions.onPostMakeDmg.RemoveSubscription(OnPostMakeDmg);
        }

    }

}
