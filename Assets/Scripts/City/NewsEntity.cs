using System;
using System.Linq;
using Assets.Scripts.DungeonGenerator.Events;
using UnityEngine;

namespace Assets.Scripts.City {

    [CreateAssetMenu(menuName = "City/News")]
    public class NewsEntity : ScriptableObject {

        public string textKey;

        public string headerKey;

        public NewsCondition[] newsConditionOr;

    }

    [Serializable]
    public class NewsCondition {

        public EventStage[] stageCompleted;

        public bool isCheckWinYear;

        public bool isWinYear;

        public bool Check()
            => stageCompleted.All(s => StaticData.current.IsCompletedStage(s)) 
                && CityMng.current.cityData.GetCurrentDungeonComplete().isWin == isWinYear;

    }

}
