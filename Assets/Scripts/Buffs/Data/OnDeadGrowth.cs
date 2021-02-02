namespace Assets.Scripts.Buffs.Data {

    public class OnDeadGrowth : Buff {

        public void OnDie() {

            foreach (var buff in fromCharacterCtrl.characterData.buffsContainer) {

                if (buff is GrowthStats growthStats)
                    growthStats.IncrementGrowth();

            }

        }

        protected override void Apply() {
            characterCtrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Buff, OnDie);
        }

        protected override void DeApply() {
            characterCtrl.characterData.stats.isDie.onPostChange.RemoveSubscription(OnDie);
        }

    }

}
