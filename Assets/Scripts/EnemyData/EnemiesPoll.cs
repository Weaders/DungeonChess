using System;
using Assets.Scripts.Character;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Fight.PlaceStrategy;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Enemies/Poll")]
    public class EnemiesPoll : ScriptableObject {

        public EnemyData[] enemies;

        public EnemyTeam[] teams;

    }


    public enum EnemyTeamStrtg { 
        MaxDistance,
        TankCarryFormation
    }

    public static class EnemyTeamStrtgExtension {

        public static TeamPlaceStrategy GetStrgObj(this EnemyTeamStrtg strtg) {

            switch (strtg) {
                case EnemyTeamStrtg.MaxDistance:
                    return new MaxDistancePlaceStrategy();
                case EnemyTeamStrtg.TankCarryFormation:
                    return new TankCarryFormationPlaceStrategy();
                default:
                    throw new GameException("Bad strtg");
            }
        
        }

    }

    [Serializable]
    public class EnemyTeam {

        public EnemyTeamStrtg enemyTeamStrtg;
        public CharacterCtrl[] characterCtrls;

    }

    [Serializable]
    public class EnemyData {

        public CharacterCtrl characterCtrl;




    }

}
