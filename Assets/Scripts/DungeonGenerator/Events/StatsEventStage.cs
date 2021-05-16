using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.StatsData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.Events {

    [CreateAssetMenu(menuName = "Events/Stats")]
    public class StatsEventStage : EventStage {

        public StatField[] fields;

        public EventStage next;

        public override void Exec() {

            var characters = GameMng.current.fightMng.fightTeamPlayer.aliveChars;

            foreach (var character in characters) {

                foreach (var field in fields) {
                    character.characterData.stats.Modify(field);
                }

            }

            if (next != null)
                next.Exec();            

        }
    }
}
