using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Buffs {
    public static class FullTeamBuffMng {

        private static List<Buff> addedBuffs = new List<Buff>();

        public static void Recalc() {

            var currentBuffsToAdd = new List<Buff>();

            foreach (var ctrl in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {

                var buffs = ctrl.characterData.buffsContainer.Where(b => b.isFullTeamBuff).ToArray();
                currentBuffsToAdd.AddRange(buffs);

            }

            var buffsForRemove = addedBuffs.Except(currentBuffsToAdd, new BuffByIdComparer());

            foreach (var buff in buffsForRemove) {

                foreach (var ctrl in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {
                    ctrl.characterData.buffsContainer.Remove(buff.GetId());
                }

            }

            //var buffsForAdd = currentBuffsToAdd.Except(addedBuffs, new BuffByIdComparer());

            foreach (var buff in currentBuffsToAdd) {

                foreach (var ctrl in GameMng.current.fightMng.fightTeamPlayer.aliveChars) {

                    var b = UnityEngine.Object.Instantiate(buff);
                    b.isFullTeamBuff = false;

                    ctrl.characterData.buffsContainer.Add(b, null, true);
                }

            }

            addedBuffs = currentBuffsToAdd;

        }

    }
}
