using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Logging;
using static Assets.Scripts.EnemyData.DungeonData;

namespace Assets.Scripts.DungeonGenerator {
    public class RoomDataGenerator {

        private readonly RoomCtrl _roomCtrl;

        private readonly DungeonData _dungeonData;

        #region State
        private List<int> healRoomLvls = new List<int>();
        private List<int> itemsRoomLvls = new List<int>();
        private Dictionary<Type, int> roomLvls = new Dictionary<Type, int>();
        private float currentHealChance = 0f;
        #endregion

        private RoomGenerator[] generators;

        public RoomDataGenerator(RoomCtrl roomCtrl, DungeonData dungeonData) {

            _roomCtrl = roomCtrl;
            _dungeonData = dungeonData;

            generators = new RoomGenerator[] 
            {
                new RoomGenerator(_dungeonData.healerRoom, new HealerRoomData("healer_room")),
                new RoomGenerator(_dungeonData.itemsRoom, new ItemsRoomData("items_room")),
                new RoomGenerator(_dungeonData.rerollRoom, new RerollRoomData("reroll_room")),
                new RoomGenerator(_dungeonData.sellerRoom, new SellerRoomData("seller_room")),
                new RoomGenerator(_dungeonData.fakeItemsRoom, new FakeItemsRoom("items_room")),
                new RoomGenerator(_dungeonData.eventRoom, new EventRoomData("event_room"))
            };

            currentHealChance = _dungeonData.healerRoom.current;

        }

        public void GenerateExit(Direction direction) {

            var roomData = GenerateExit();
            _roomCtrl.currentRoom.roomData.AddExit(roomData, direction);

        }

        public RoomData GenerateStartRoom() {
            return new StartRoomData("start_room");
        }

        private RoomData GenerateExit() {

            var newLvl = GameMng.current.level + 1;

            var random = UnityEngine.Random.value;

            #region Boss Room

            //TagLogger<RoomDataGenerator>.Info($"Count levels - {GameMng.current.countLevels}. Current lvl - {newLvl}");

            if (GameMng.current.IsNextBossRoom()) {

                TagLogger<RoomDataGenerator>.Info($"Generate boss room");
                return new BossRoomData("boss_room");

            }

            #endregion

            #region Build Room

            if (GameMng.current.IsNextBuildRoom()) {

                TagLogger<RoomDataGenerator>.Info("Generate build room");
                return new RerollRoomData("reroll_room");

            }

            #endregion

            #region Generators

            RoomData selectedRoom = null;

            foreach (var generator in generators.OrderByDescending(g => g.currentChance)) {

                if (selectedRoom == null)
                    selectedRoom = generator.TryGen(newLvl, random);
                else
                    generator.PlusChance();

            }

            if (selectedRoom != null)
                return selectedRoom;

            #endregion

            #region Enemy room
            
            TagLogger<RoomDataGenerator>.Info($"Generate enenmy room");
            return new EnemyRoomData("one_more_room");

            #endregion


        }

        public class RoomGenerator {

            private RoomDataAndChance _ch;

            private List<int> lvls = new List<int>();

            private float _currentChance = 0;

            public float currentChance => _currentChance;

            private RoomData _prototype;

            public RoomGenerator(RoomDataAndChance ch, RoomData prototype) {

                _ch = ch;
                _currentChance = _ch.current;
                _prototype = prototype;

            }

            public RoomData TryGen(int newLvl, float random) {

                var maxLvl = lvls.Any() ? lvls.Max() : -_ch.delay;

                if (maxLvl + _ch.delay < newLvl) {

                    if (random < _currentChance) {

                        lvls.Add(newLvl);
                        _currentChance = _ch.start;

                        TagLogger<RoomDataGenerator>.Info($"Generate {_prototype.GetType().Name}");

                        return _prototype.Clone() as RoomData;

                    } else {
                        PlusChance();
                    }

                }

                return null;

            }

            public void PlusChance() {
                _currentChance += _ch.toAdd;
            }

        }

    }


}
