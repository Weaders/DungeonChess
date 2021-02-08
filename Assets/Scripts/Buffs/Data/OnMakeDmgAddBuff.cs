using System.Linq;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Character;
using Assets.Scripts.Translate;
using static Assets.Scripts.ActionsData.Actions;

namespace Assets.Scripts.Buffs.Data {

    public class OnMakeDmgAddBuff : Buff {

        public Buff buffToAddPrefab;        

        protected override void Apply() {
            characterCtrl.characterData.actions.onPostMakeDmg.AddSubscription(Observable.OrderVal.Buff, OnPostMakeDmg);
        }

        private void OnPostMakeDmg(DmgEventData dmgEventData) {

            if (buffToAddPrefab is IDmgSource source) {

                if (dmgEventData.dmg.source != null && dmgEventData.dmg.source.GetId() != source.GetId()) {
                    dmgEventData.to.characterData.buffsContainer.AddPrefab(buffToAddPrefab, characterCtrl);
                }

            } else {
                dmgEventData.to.characterData.buffsContainer.AddPrefab(buffToAddPrefab, characterCtrl);
            }
            
        }

        protected override void DeApply() {
            characterCtrl.characterData.actions.onPostMakeDmg.RemoveSubscription(OnPostMakeDmg);
        }

        protected override Placeholder[] GetPlaceholders(CharacterData descriptionFor) {

            return buffToAddPrefab
                .GetPlaceholdersFromAttrs(descriptionFor)
                .Concat(base.GetPlaceholders(descriptionFor))
                .ToArray();

        }

    }

}
