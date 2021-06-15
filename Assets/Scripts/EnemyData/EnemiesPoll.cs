using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Fight.PlaceStrategy;
using Assets.Scripts.Observable;
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

        public EnemyTeam GetEnemyTeam(int lvl)
            => teams.FirstOrDefault(t => t.forceForRoom == lvl);
    }

    public enum EnemyTeamStrtg {
        MaxDistance,
        TankCarryFormation,
        MiddleFormation,
        Preferred
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
                case EnemyTeamStrtg.Preferred:
                    return new PreferStrategy();
                default:
                    throw new GameException("Bad strtg");
            }

        }

    }

    [Serializable]
    public class EnemyTeam {

        public bool usePrefferedStrtg = true;
        public EnemyTeamStrtg enemyTeamStrtg;
        public CharacterDungeonData[] characterCtrls;
        public bool isBoss;
        public bool isEnabled;
        public Condition difficult;
        public int forceForRoom = -1;

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

        public ModifyType modifyType => levelOfDifficult == -1 ? ModifyType.Plus : ModifyType.Set;

    }

}
