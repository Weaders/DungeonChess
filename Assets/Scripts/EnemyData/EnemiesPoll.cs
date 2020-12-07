using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Fight.PlaceStrategy;
using UnityEngine;
using static Assets.Scripts.EnemyData.DungeonData;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Enemies/Poll")]
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
        public CharacterCtrl[] characterCtrls;
        public bool isBoss;
        public bool isEnabled;
        public Condition condition;

    }

    [Serializable]
    public class Condition : RangeRooms {
    }

}
