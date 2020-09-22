using Assets.Scripts.ActionsData;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Buffs.Data {

    public class OnMakeDmgAddBuff : Buff {

        public Buff buffToAddPrefab;        

        protected override void Apply() {
            characterCtrl.characterData.actions.onPostMakeDmg.AddSubscription(Observable.OrderVal.Buff, OnPostMakeDmg);
        }

        private void OnPostMakeDmg(DmgEventData dmgEventData) {

            if (buffToAddPrefab is IDmgSource source) {

                if (dmgEventData.dmg.source.GetId() != source.GetId()) {
                    dmgEventData.to.characterData.buffsContainer.AddPrefab(buffToAddPrefab, characterCtrl);
                }

            } else {
                dmgEventData.to.characterData.buffsContainer.AddPrefab(buffToAddPrefab, characterCtrl);
            }
            
        }

        protected override void DeApply() {
            characterCtrl.characterData.actions.onPostMakeDmg.RemoveSubscription(OnPostMakeDmg);
        }

    }

}
