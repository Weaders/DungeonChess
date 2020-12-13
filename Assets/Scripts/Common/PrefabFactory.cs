using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;

namespace Assets.Scripts.Common {

    public static class PrefabFactory {

        static int characterCount = 0;

        public static CharacterCtrl InitCharacterCtrl(CharacterCtrl ctrlPrefab) {

            var ctrl = Object.Instantiate(ctrlPrefab);

            ctrl.gameObject.name = $"{ctrl.gameObject.name}_{++characterCount}";

            ctrl.Init();

            return ctrl;

        }

        public static DungeonRoomCells InitRoomCells(DungeonRoomCells prefab, RoomData roomData) {

            var room = Object.Instantiate(prefab);

            room.roomData = roomData;

            return room;

        }


    }
}
