using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Translate;
using UnityEngine;
using static Assets.Scripts.Fight.PlaceStrategy.TeamPlaceStrategy;

namespace Assets.Scripts.DungeonGenerator {
    public class EnemyRoomData : RoomData {

        public EnemyRoomData(string titleKey) : base(titleKey) {
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            var teams = GetEnemyTeams();

            if (teams.Any()) {

                var teamIndex = Random.Range(0, teams.Length);
                var team = teams[teamIndex];

                foreach (var ctrlData in team.characterCtrls) {

                    var ctrlObj = PrefabFactory.InitCharacterCtrl(ctrlData.characterCtrl, true);
                    GameMng.current.fightMng.fightTeamEnemy.AddCharacterToTeam(ctrlObj);

                }

                var preferred = EnemyTeamStrtg.Preferred.GetStrgObj();

                preferred.Place(
                    GameMng.current.fightMng.fightTeamEnemy,
                    GameMng.current.cellsGridMng.enemiesSideCell.Where(c => c.IsAvailableToStay()),
                    GameMng.current.cellsGridMng.playerSideCell
                );

                var strtg = team.enemyTeamStrtg.GetStrgObj();

                strtg.Place(
                    GameMng.current.fightMng.fightTeamEnemy.aliveChars.Where(c => c.characterData.cell == null),
                    GameMng.current.cellsGridMng.enemiesSideCell.Where(c => c.IsAvailableToStay()),
                    GameMng.current.cellsGridMng.playerSideCell
                );

                GameMng.current.fightMng.onPlayerWin.AddListener(OnPlayerWin);

            }

            GameMng.current.topSidePanelUI.stateTopBtn = TopSidePanel.StateTopBtn.Start;

        }

        protected virtual void OnPlayerWin() {

            GameMng.current.cellsGridMng.DisplayExits();

            GameMng.current.fightMng.onPlayerWin.RemoveListener(OnPlayerWin);

        }

        protected virtual EnemyTeam[] GetEnemyTeams() {

            var lvl = GameMng.current.level;

            var team = GameMng.current.currentDungeonData.enemiesPoll.GetEnemyTeam(lvl);

            if (team != null)
                return new[] { team };

            var enemies = GameMng.current.currentDungeonData.enemiesPoll
                .GetStandartEnemies()
                .Where(t => t.difficult.IsInRange(GameMng.current.levelDifficult))
                .ToArray();

            return enemies;
        }

        public override object Clone()
            => new EnemyRoomData(_title);

        public override Sprite GetSprite()
            => GameMng.current.currentDungeonData.enemyRoom.roomSprite;

        public override string GetRoomDescription()
            => TranslateReader.GetTranslate("enemies_room_description");
    }
}
