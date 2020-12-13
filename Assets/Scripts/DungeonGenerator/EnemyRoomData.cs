﻿using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator {
    public class EnemyRoomData : RoomData {

        public EnemyRoomData(string titleKey) : base(titleKey) {
        }

        public override void ComeToRoom(DungeonRoomCells dungeonRoomCells) {

            var teams = GetEnemyTeams();

            if (teams.Any()) {

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

                GameMng.current.fightMng.onPlayerWin.AddListener(OnPlayerWin);

            } else {
            
            }

        }

        private void OnPlayerWin() {

            GameMng.current.playerData.money.val += GameMng.current.currentDungeonData.moneyVictory;

            GameMng.current.cellsGridMng.DisplayExits();

            GameMng.current.fightMng.onPlayerWin.RemoveListener(OnPlayerWin);

        }

        protected virtual EnemyTeam[] GetEnemyTeams() {

            var enemies = GameMng.current.currentDungeonData.enemiesPoll.GetStandartEnemies()
                .Where(e => e.condition.IsInRange(GameMng.current.level))
                .ToArray();

            return enemies;
        }

        public override object Clone()
            => new EnemyRoomData(_title);
    }
}
