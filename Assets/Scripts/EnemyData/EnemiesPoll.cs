using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Fight.PlaceStrategy;
using Assets.Scripts.StatsData;
using UnityEngine;
using static Assets.Scripts.EnemyData.DungeonData;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/EnemiesPoll")]
    public class EnemiesPoll : ScriptableObject {

        public EnemyTeam[] teams;

        public IEnumerable<EnemyTeam> GetBossTeams()
            => teams.Where(t => t.isBoss && t.isEnabled);

        public IEnumerable<EnemyTeam> GetStandartEnemies()
            => teams.Where(t => !t.isBoss && t.isEnabled);
    }

    public enum EnemyTeamStrtg {
        MaxDistance,
        TankCarryFormation,
        MiddleFormation
    }

    public static class EnemyTeamStrtgExtension {

        public static TeamPlaceStrategy GetStrgObj(this EnemyTeamStrtg strtg) {

            switch (strtg) {
                case EnemyTeamStrtg.MaxDistance:
                    return new MaxDistancePlaceStrategy();
                case EnemyTeamStrtg.TankCarryFormation:
                    return new TankCarryFormationPlaceStrategy();
                case EnemyTeamStrtg.MiddleFormation:
                    return new MiddleFormation();
                default:
                    throw new GameException("Bad strtg");
            }

        }

    }

    [Serializable]
    public class EnemyTeam {

        public EnemyTeamStrtg enemyTeamStrtg;
        public CharacterDungeonData[] characterCtrls;
        public bool isBoss;
        public bool isEnabled;

    }

    [Serializable]
    public class Condition : RangeRooms {
    }

    [Serializable]
    public class StatGroup {

        public StatField[] stats;

        /// <summary>
        /// Start from 0
        /// </summary>
        public int levelOfDifficult;

    }

}
