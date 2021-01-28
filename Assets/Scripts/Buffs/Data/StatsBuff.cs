using Assets.Scripts.StatsData;

namespace Assets.Scripts.Buffs.Data {

    public class StatsBuff : Buff {

        public StatField[] statsModify;

        protected override void Apply() {

            foreach (var stat in statsModify)
                characterCtrl.characterData.stats.Mofify(stat);

        }

        protected override void DeApply() {

            foreach (var stat in statsModify)
                characterCtrl.characterData.stats.Mofify(stat, Observable.ModifyType.Minus);

        }
    }
}
