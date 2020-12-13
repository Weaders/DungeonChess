using System;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/Data")]
    public class DungeonData : ScriptableObject {

        public EnemiesPoll enemiesPoll;

        public int moneyVictory;

        public RangeRooms countRooms;

        public RoomPrefabsData[] dungeonRoomCells;

        public RoomChance healerRoom;

        public RoomChance itemsRoom;

        public RoomChance rerollRoom;

        public DungeonRoomCells GetRoomForLvlPrefab(int lvl, bool isBoss = false) {

            var cells = dungeonRoomCells
                .Where(r => r.whereData.IsInRange(lvl));

            if (isBoss)
                cells = cells.Where(c => c.isBoss);

            return cells
                .Select(r => r.roomPrefab)
                .RandomElement();

        }

        public Vector2Int GetRoomSize()
            => new Vector2Int(6, 6);

        [Serializable]
        public class RangeRooms {

            public int min;
            public int max;

            public bool IsInRange(int val)
                => min <= val && max >= val;

            public int GetRandomLvl()
                => UnityEngine.Random.Range(min, max);

        }

        [Serializable]
        public class RoomPrefabsData {

            public DungeonRoomCells roomPrefab;
            public RangeRooms whereData;
            public bool isBoss;

        }

        [Serializable]
        public class RoomChance {

            public float start;
            public float current;
            public float toAdd;
            public float delay;

        }

    }

}
