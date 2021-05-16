using System;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.City {
    public class CityData : MonoBehaviour {

        public NewsList[] newsList;

        public DungeonYear[] dungeonYears;

        public DungeonComplete[] dungeonCompletes;

        public int maxViolence = 100;

        public ObservableVal<int> violence { get; set; } = new ObservableVal<int>();

        public ObservableVal<float> demonScience { get; set; } = new ObservableVal<float>();

        public ObservableVal<int> necroApprove { get; set; } = new ObservableVal<int>();

        public bool[] dungeonYearsIsWin;

        private int currentDungeon = 0;

        public int GetCurrentDungeonIndex()
            => currentDungeon;

        private void Awake() {
            
            currentDungeon = StaticData.current.GetLastCompleteDungeon() + 1;
            dungeonYearsIsWin = new bool[dungeonYears.Length];
            dungeonCompletes = new DungeonComplete[dungeonYears.Length];

        }

        public DungeonData GetNextDungeon() {

            if (currentDungeon > 0)
                return dungeonYears[currentDungeon].dungeonData;

            return null;

        }

        public NewsList GetCurrentNews() {

            if (currentDungeon > 0)
                return newsList[currentDungeon - 1];

            return null;

        }

        public void ApplyChanges(CityDataChange cityDataChange) {

            violence.val += cityDataChange.violenceOffset;
            necroApprove.val += cityDataChange.necroApproveOffset;
            demonScience.val += cityDataChange.demonScienceOffset;

        }

        public void AddDungeonComplete(DungeonComplete dungeonComplete) {

            dungeonCompletes[GetCurrentDungeonIndex()] = dungeonComplete;

        }

        public DungeonComplete GetCurrentDungeonComplete()
            => dungeonCompletes[GetCurrentDungeonIndex()];

    }

    [Serializable]
    public class DungeonYear {

        public int year;
        public DungeonData dungeonData;

    }

    public class CityDataChange {

        public int violenceOffset { get; set; }

        public float demonScienceOffset { get; set; }

        public int necroApproveOffset { get; set; }

    }

    public class DungeonComplete {

        public bool isWin;

    }
}
