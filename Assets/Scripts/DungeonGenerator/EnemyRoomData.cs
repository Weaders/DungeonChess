using System.Linq;
using Assets.Scripts.CellsGrid;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Translate;
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

                foreach (var ctrlData in team.characterCtrls) {

                    var ctrlObj = PrefabFactory.InitCharacterCtrl(ctrlData.characterCtrl);

                    ctrlObj.gameObject.name = $"EnemyCtrl_{i}";

                    ctrlObj.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Internal, () => {
                        GameMng.current.playerData.money.val += GameMng.current.currentDungeonData.moneyVictory;
                    });

                    GameMng.current.fightMng.fightTeamEnemy.AddCharacterToTeam(ctrlObj);

                    var statsChange = ctrlData.data.FirstOrDefault(d => d.levelOfDifficult == GameMng.current.levelDifficult);

                    if (statsChange == null) {
                        statsChange = ctrlData.data.Last();
                    }

                    if (statsChange != null) {

                        foreach (var stat in statsChange.stats) {
                            ctrlObj.characterData.stats.Mofify(stat, Observable.ModifyType.Set);
                        }

                    }

                    ++i;

                }

                var strtg = team.enemyTeamStrtg.GetStrgObj();

                strtg.Place(
                    GameMng.current.fightMng.fightTeamEnemy,
                    GameMng.current.cellsGridMng.enemiesSideCell.Where(c => c.IsAvailableToStay()),
                    GameMng.current.cellsGridMng.playerSideCell
                );

                GameMng.current.fightMng.onPlayerWin.AddListener(OnPlayerWin);

            }

            GameMng.current.topSidePanelUI.stateTopBtn = TopSidePanel.StateTopBtn.Start;

        }

        protected virtual void OnPlayerWin() {

            GameMng.current.playerData.money.val += GameMng.current.currentDungeonData.moneyVictory;

            GameMng.current.cellsGridMng.DisplayExits();

            GameMng.current.fightMng.onPlayerWin.RemoveListener(OnPlayerWin);

        }

        protected virtual EnemyTeam[] GetEnemyTeams() {

            var enemies = GameMng.current.currentDungeonData.enemiesPoll
                .GetStandartEnemies()
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
