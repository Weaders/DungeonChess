using System;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/Data")]
    public class DungeonData : ScriptableObject {

        public EnemiesPoll enemiesPoll;

        public int moneyVictory;

        public RangeRooms countRooms;

        [Serializable]
        public class RangeRooms {
            public int min;
            public int max;
        }

        public bool isThereHealRooms;

    }

}
