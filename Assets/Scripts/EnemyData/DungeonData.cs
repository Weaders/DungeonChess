using System;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/Data")]
    public class DungeonData : ScriptableObject {

        public EnemiesPoll enemiesPoll;

        public EventsPoll eventsPoll;

        public BuffsPoll buffsPoll;

        public int moneyVictory;

        public int countRooms = 1;

        public RangeRooms changeDifficultEvery;

        public RoomPrefabsData[] dungeonRoomCells;

        public RoomDataAndChance healerRoom;

        public RoomDataAndChance itemsRoom;

        public RoomDataAndChance rerollRoom;

        public RoomDataAndChance enemyRoom;

        public RoomDataAndChance sellerRoom;

        public RoomDataAndChance fakeItemsRoom;

        public RoomDataAndChance eventRoom;

        public RoomDataAndChance healAndBuffRoom;

        public DungeonRoomCells GetRoomForLvlPrefab(int lvl, bool isBoss = false) {
            
            var cells = dungeonRoomCells.AsEnumerable();

            if (isBoss)
                cells = cells.Where(c => c.isBoss);

            return cells
                .Select(r => r.roomPrefab)
                .RandomElement();

        }

        [Serializable]
        public class RangeRooms {

            public int min;
            public int max;

            public bool IsInRange(int val)
                => min <= val && (max == -1 || max >= val);

            public int GetRandomLvl()
                => UnityEngine.Random.Range(min, max);

        }

        [Serializable]
        public class RoomPrefabsData {

            public DungeonRoomCells roomPrefab;
            public bool isBoss;

        }

        [Serializable]
        public class RoomDataAndChance {

            public float start;
            public float current;
            public float toAdd;
            public float delay;

            public Sprite roomSprite;

            public int[] forceRooms;

        }

    }

}
