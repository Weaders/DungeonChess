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

        private Dictionary<FightTeam, IEnumerable<GameObject>> addedLines = new Dictionary<FightTeam, IEnumerable<GameObject>>();

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

            if (addedLines.TryGetValue(team, out var teamLines)) {

                foreach (var obj in teamLines) {
                    Destroy(obj);
                }

                addedLines.Remove(team);

            }

            var results = Recalc(team.aliveChars);

            var ctrlBuffs = new List<(CharacterCtrl, Buff)>();
            var lines = new List<GameObject>();

            foreach (var result in results) {

                foreach (var ctrl in result.ctrls) {

                    TagLogger<SynergyCtrl>.Info($"Add buff {result.buffResult.name} to {ctrl.name}");
                    ctrlBuffs.Add((ctrl, ctrl.characterData.buffsContainer.AddPrefab(result.buffResult)));

                }

                lines.Add(GameMng.current.synergyLines.CreateLineFor(Color.green, result.ctrls.ToArray()));

            }

            addedLines.Add(team, lines);
            addedBuffsForTeam.Add(team, ctrlBuffs);

        }

    }

}
