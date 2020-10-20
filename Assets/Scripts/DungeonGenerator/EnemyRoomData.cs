using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class EnemyRoomData : RoomData {

        public EnemyRoomData(Vector2Int sizeData) : base(sizeData) {
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            var i = 0;

            var enemiesPoll = GameMng.current.currentDungeonData.enemiesPoll;

            var teamIndex = UnityEngine.Random.Range(0, enemiesPoll.teams.Length);

            var team = enemiesPoll.teams[teamIndex];

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
