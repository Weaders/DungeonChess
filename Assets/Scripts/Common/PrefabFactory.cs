using Assets.Scripts.CellsGrid;
using Assets.Scripts.Character;
using Assets.Scripts.DungeonGenerator;
using UnityEngine;

namespace Assets.Scripts.Common {

    public static class PrefabFactory {

        static int characterCount = 0;

        public static CharacterCtrl InitCharacterCtrl(CharacterCtrl ctrlPrefab, bool isEnemy = false) {

            var ctrl = Object.Instantiate(ctrlPrefab);

            ctrl.gameObject.name = $"{ctrl.gameObject.name}_{++characterCount}";

            ctrl.Init();

            if (isEnemy) {

                ctrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Internal, () => {

                    // GameMng.current.fightTextMng.DisplayText(
                    //     ctrl, 
                    //     GameMng.current.currentDungeonData.moneyVictory.ToString(), 
                    //     new FightText.FightTextMsg.SetTextOpts {
                    //         color = StaticData.current.colorStore.getMoneyText,
                    //         icon = GameMng.current.gameData.playerManaIcon
                    //     }
                    // );

                    GameMng.current.playerData.money.val += GameMng.current.currentDungeonData.moneyVictory;

                });

            }

            return ctrl;

        }

        public static DungeonRoomCells InitRoomCells(DungeonRoomCells prefab, RoomData roomData) {

            var room = Object.Instantiate(prefab);

            room.roomData = roomData;

            return room;

        }


    }
}
