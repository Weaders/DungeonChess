using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.StatsData;
using static Assets.Scripts.State.StateData;

namespace Assets.Scripts.Buffs.Data {
    public class StatToStatBuff : Buff {

        public StatField minusFrom;
        public StatField addTo;

        //public StatField percentFrom;
        //public StatField percentTo;

        protected override void Apply() {

            if (minusFrom.statType != Stat.None) {

                characterCtrl.characterData.stats.Modify(minusFrom, Observable.ModifyType.Minus);
                characterCtrl.characterData.stats.Modify(addTo, Observable.ModifyType.Plus);
            }
            //} else if (percentFrom.statType != Stat.None) {

            //    //var stat = characterCtrl.characterData.stats.GetStat(percentFrom.stat);

            //    //characterCtrl.characterData.stats.Modify(percentFrom, Observable.ModifyType.Minus);
            //    //characterCtrl.characterData.stats.Modify(addTo, Observable.ModifyType.Plus);

            //}

        }

        protected override void DeApply() {

            if (minusFrom.statType != Stat.None) {

                characterCtrl.characterData.stats.Modify(minusFrom, Observable.ModifyType.Plus);
                characterCtrl.characterData.stats.Modify(addTo, Observable.ModifyType.Minus);

            }
            //} else if (percentFrom.statType != Stat.None) {

            //    characterCtrl.characterData.stats.Modify(minusFrom, Observable.ModifyType.Set);
            //    characterCtrl.characterData.stats.Modify(addTo, Observable.ModifyType.Set);

            //}

        }
    }
}
