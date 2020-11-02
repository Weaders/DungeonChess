using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Logging;

namespace Assets.Scripts.DungeonGenerator {
    public class RoomDataGenerator {

        private readonly RoomCtrl _roomCtrl;

        private readonly DungeonData _dungeonData;

        #region State
        private List<int> healRoomLvls = new List<int>();
        private float currentHealChance = 0f;
        #endregion

        public RoomDataGenerator(RoomCtrl roomCtrl, DungeonData dungeonData) {

            _roomCtrl = roomCtrl;
            _dungeonData = dungeonData;

            currentHealChance = _dungeonData.healerRoom.current;

        }

        public void GenerateExit(Direction direction) {

            var roomData = GenerateExit();
            _roomCtrl.currentRoom.roomData.AddExit(roomData, direction);

        }

        public RoomData GenerateStartRoom() {
            return new StartRoomData(_dungeonData.GetRoomSize());
        }

        private RoomData GenerateExit() {

            var chances = new[] {
                _dungeonData.healerRoom
            };

            var newLvl = GameMng.current.level + 1;

            var random = UnityEngine.Random.value;

            #region Heal Room

            var maxLvl = healRoomLvls.Any() ? healRoomLvls.Max() : (_dungeonData.healerRoom.delay + 1 + newLvl);

            if (maxLvl - newLvl > _dungeonData.healerRoom.delay) {

                if (random < currentHealChance) {

                    healRoomLvls.Add(newLvl);
                    currentHealChance = _dungeonData.healerRoom.start;

                    TagLogger<RoomDataGenerator>.Info($"Generate heal room");

                    return new HealerRoomData(_dungeonData.GetRoomSize());

                } else {
                    currentHealChance += _dungeonData.healerRoom.toAdd;
                }

            }

            #endregion

            TagLogger<RoomDataGenerator>.Info($"Generate enenmy room");

            return new EnemyRoomData(_dungeonData.GetRoomSize());

        }

    }
}
