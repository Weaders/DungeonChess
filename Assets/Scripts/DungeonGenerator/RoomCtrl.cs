using System;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DungeonGenerator {

    public class RoomCtrl : MonoBehaviour {

        private RoomDataGenerator roomDataGenerator;

        private DungeonRoomCells _currentRoomCells = null;

        public DungeonRoomCells currentRoom {
            get => _currentRoomCells;
            private set {

                _currentRoomCells = value;
                onMoveToNextRoom.Invoke();

            }
        }

        public Rect XZRect { get; private set; }

        public UnityEvent onMoveToNextRoom = new UnityEvent();

        [SerializeField]
        private float durationPopupTitle = 1.5f;

        public void Init() {
            roomDataGenerator = new RoomDataGenerator(this, GameMng.current.currentDungeonData);
        }

        public void MoveToStartRoom() {

            if (currentRoom != null) {

                Destroy(currentRoom.gameObject);
                currentRoom = null;

            }

            var startRoomData = roomDataGenerator.GenerateStartRoom();

            currentRoom = PrefabFactory.InitRoomCells(
                GameMng.current.currentDungeonData.GetRoomForLvlPrefab(GameMng.current.level, false),
                startRoomData
            );

            ProcessRoom();

        }

        public void MoveToNextRoom(Direction direction) {

            GameMng.current.ShowBlackOverlay();

            var timeForHide = Time.time + 1.5f;

            var roomData = currentRoom.roomData.exitFromRooms.First(e => e.direction == direction).toRoomData;

            if (currentRoom != null) {

                Destroy(currentRoom.gameObject);
                currentRoom = null;

            }

            if (roomData is BossRoomData boosRoomData) {

                currentRoom = PrefabFactory.InitRoomCells(
                    GameMng.current.currentDungeonData.GetRoomForLvlPrefab(GameMng.current.level, true),
                    roomData
                );

            } else {

                currentRoom = PrefabFactory.InitRoomCells(
                    GameMng.current.currentDungeonData.GetRoomForLvlPrefab(GameMng.current.level),
                    roomData
                );

            }

            GameMng.current.level.val += 1;
            GameMng.current.levelsBeforeChange.val--;

            ProcessRoom();

            GameMng.current.HideBlackOverlay();
      
        }

        public Rect RecalcRect() {

            Vector3? minXZ = null, maxXZ = null;

            foreach (Transform tr in currentRoom.transform) {

                var xz = tr.position.x + tr.position.z;

                if (!minXZ.HasValue)
                    minXZ = tr.position;
                else
                    minXZ = xz < minXZ.Value.x + minXZ.Value.z ? tr.position : minXZ;
                if (!maxXZ.HasValue)
                    maxXZ = tr.position;
                else
                    maxXZ = xz > maxXZ.Value.x + maxXZ.Value.z ? tr.position : maxXZ;

            }

            return new Rect(minXZ.Value.x, minXZ.Value.z, maxXZ.Value.x - minXZ.Value.x, maxXZ.Value.z - minXZ.Value.z);

        }

        private void ProcessRoom() {

            currentRoom.transform.SetParent(transform);
            currentRoom.transform.localPosition = Vector3.zero;
            GameMng.current.isBuildPhase.val = true;

            var directions = Enum.GetValues(typeof(Direction));

            var countExists = UnityEngine.Random.Range(2, directions.Length);

            if (GameMng.current.IsNextBossRoom()) {
                roomDataGenerator.GenerateExit(Direction.top);
            } else if (GameMng.current.IsNextBuildRoom()) {
                roomDataGenerator.GenerateExit(Direction.top);
            } else {

                for (var i = 0; i < countExists; i++) {
                    roomDataGenerator.GenerateExit((Direction)directions.GetValue(i));
                }

            }

            TagLogger<RoomCtrl>.Info($"Generate {countExists} exits");

            GameMng.current.cellsGridMng.RefreshCells();

            currentRoom.roomData.ComeToRoom(currentRoom);

            if (!(currentRoom.roomData is StartRoomData)) {
                GameMng.current.locationTitle.ShowPopup(currentRoom.roomData.title, durationPopupTitle);
            }

            foreach (var cell in currentRoom.GetCells()) {
                cell.AddState(Cell.CellState.Select);
            }

            XZRect = RecalcRect();

            GameMng.current.fightMng.MovePlayerCtrls();

        }

    }

}
