using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Buffs;
using Assets.Scripts.Character;
using Assets.Scripts.Fight;
using Assets.Scripts.Logging;
using Assets.Scripts.Synergy;
using UnityEngine;

namespace Assets.Scripts {

    public class SynergyCtrl : MonoBehaviour {

        public SynergyData[] synergyDatas;

        private Dictionary<FightTeam, List<(CharacterCtrl, Buff)>> addedBuffsForTeam = new Dictionary<FightTeam, List<(CharacterCtrl, Buff)>>();

        public RecalcResult[] Recalc(IEnumerable<CharacterCtrl> ctrls) {

            IEnumerable<RecalcResult> resultArray = new RecalcResult[0];

            foreach (var sd in synergyDatas) {

                var result = sd.Recalc(ctrls);

                if (result != null) {
                    resultArray = resultArray.Union(result.Where(r => r != null));
                }

            }

            return resultArray.ToArray();

        }

        public void SetUpTeam(FightTeam team) {

            if (addedBuffsForTeam.TryGetValue(team, out var ctrlBufffs)) {

                foreach (var (ctrl, addedBuff) in ctrlBufffs) {
                    ctrl.characterData.buffsContainer.Remove(addedBuff);
                }

                addedBuffsForTeam.Remove(team);

            }

            var results = Recalc(team.aliveChars);

            var ctrlBuffs = new List<(CharacterCtrl, Buff)>();

            foreach (var result in results) {

                foreach (var ctrl in result.ctrls) {

                    TagLogger<SynergyCtrl>.Info($"Add buff {result.buffResult.name} to {ctrl.name}");
                    ctrlBuffs.Add((ctrl, ctrl.characterData.buffsContainer.AddPrefab(result.buffResult)));

                }

                GameMng.current.synergyLines.GetPointFor(result.synergyData, result.ctrls.ToArray());

            }

            addedBuffsForTeam.Add(team, ctrlBuffs);

        }

    }

}
