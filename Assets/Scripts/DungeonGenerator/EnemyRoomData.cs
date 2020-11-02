using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class EnemyRoomData : RoomData {

        public EnemyRoomData(Vector2Int sizeData) : base(sizeData) {
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            EnemyTeam[] teams;

            if (GameMng.current.countRooms - 1 == GameMng.current.level) {
                teams = GameMng.current.currentDungeonData.enemiesPoll.GetBossTeams().ToArray();
            } else {
                teams = GameMng.current.currentDungeonData.enemiesPoll.GetStandartEnemies().ToArray();
            }

            var teamIndex = Random.Range(0, teams.Length);
            var team = teams[teamIndex];

            var i = 0;

            foreach (var ctrl in team.characterCtrls) {

                var ctrlObj = PrefabFactory.InitCharacterCtrl(ctrl);

                ctrlObj.gameObject.name = $"EnemyCtrl_{i}";

                GameMng.current.fightMng.fightTeamEnemy.AddCharacterToTeam(ctrlObj);

                ++i;

            }

            var strtg = team.enemyTeamStrtg.GetStrgObj();

            strtg.Place(
                GameMng.current.fightMng.fightTeamEnemy,
                GameMng.current.cellsGridMng.enemiesSideCell,
                GameMng.current.cellsGridMng.playerSideCell
            );

        }
    }
}
