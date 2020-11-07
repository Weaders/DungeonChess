using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.DungeonGenerator {

    public class RoomCtrl : MonoBehaviour {

        private RoomDataGenerator roomDataGenerator;

        public DungeonRoomCells currentRoom { get; private set; }

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

            ProcessRoom();

            GameMng.current.HideBlackOverlay();

        }

        private void ProcessRoom() {

            currentRoom.transform.SetParent(transform);
            currentRoom.transform.localPosition = Vector3.zero;

            foreach (var exit in currentRoom.GetExits()) {
                roomDataGenerator.GenerateExit(exit.exitDirection);
            }

            GameMng.current.cellsGridMng.RefreshCells();

            currentRoom.roomData.ComeToRoom(currentRoom);

            GameMng.current.locationTitle.ShowPopup(currentRoom.roomData.title, durationPopupTitle);

            GameMng.current.fightMng.MovePlayerCtrls();

            var surfaces = GetComponents<NavMeshSurface>();

            foreach (var surface in surfaces)
                surface.BuildNavMesh();

        }

    }

}
