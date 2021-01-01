using System;
using System.Collections;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
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

            ProcessRoom();

            //if (timeForHide <= Time.time) {
                GameMng.current.HideBlackOverlay();
            //} else {

            //    IEnumerator hideForDiff() {
            //        yield return new WaitForSeconds(timeForHide - Time.time);
            //        GameMng.current.HideBlackOverlay();
            //    }

            //    StartCoroutine(hideForDiff());

            //}            

        }

        private void ProcessRoom() {

            currentRoom.transform.SetParent(transform);
            currentRoom.transform.localPosition = Vector3.zero;

            var directions = Enum.GetValues(typeof(Direction));

            var countExists = UnityEngine.Random.Range(1, directions.Length);

            for (var i = 0; i < countExists; i++) {
                roomDataGenerator.GenerateExit((Direction)directions.GetValue(i));
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

            GameMng.current.fightMng.MovePlayerCtrls();


            //var surfaces = GetComponents<NavMeshSurface>();

            //foreach (var surface in surfaces)
            //    surface.BuildNavMesh();

        }

    }

}
