using System;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator.Events;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {

    [CreateAssetMenu(menuName = "Dungeon/EventsPoll")]
    public class EventsPoll : ScriptableObject {

        public EventData[] events;

        [Serializable]
        public class EventData {

            public EventStage startStage;

        }

        public EventData GetRandomEvent()
            => events.RandomElement();

    }
}
