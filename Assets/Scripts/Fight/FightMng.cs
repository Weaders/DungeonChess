using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.EnemyData;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Fight {

    public enum TeamSide {
        Player, Enemy
    }

    public class FightMng : MonoBehaviour {

        public FightTeam fightTeamPlayer = new FightTeam(TeamSide.Player);
        public FightTeam fightTeamEnemy = new FightTeam(TeamSide.Enemy);

        public UnityEvent onPlayerWin = new UnityEvent();
        public UnityEvent onEnemyTeamWin = new UnityEvent();

        public bool isInFight { get; set; }

        public FightTeam GetEnemyTeamFor(CharacterCtrl characterCtrl) {

            if (fightTeamPlayer.chars.Contains(characterCtrl)) {
                return fightTeamEnemy;
            } else {
                return fightTeamPlayer;
            }

        }

        public void RefreshEnemies() {

            fightTeamEnemy.ClearChars();

            var i = 0;

            var enemiesPoll = GameMng.current.currentDungeonData.enemiesPoll;

            var teamIndex = Random.Range(0, enemiesPoll.teams.Length);

            var team = enemiesPoll.teams[teamIndex];

            foreach (var ctrl in team.characterCtrls) {

                var ctrlObj = PrefabFactory.InitCharacterCtrl(ctrl);

                ctrlObj.gameObject.name = $"EnemyCtrl_{i}";

                fightTeamEnemy.AddCharacterToTeam(ctrlObj);

                ++i;
            }

            var strtg = team.enemyTeamStrtg.GetStrgObj();

            strtg.Place(
                fightTeamEnemy, 
                GameMng.current.cellsGridMng.enemiesSideCell,
                GameMng.current.cellsGridMng.playerSideCell
            );

        }

        public void MovePlayerCtrls() {

            var cells = GameMng.current.cellsGridMng.currentRoom.GetCells();

            foreach (var charCtrl in fightTeamPlayer.aliveChars) {

                foreach (var cell in cells) {

                    if (cell.dataPosition == charCtrl.cell.dataPosition) {

                        cell.StayCtrl(charCtrl);
                        break;

                    }

                }

            }

        }

        public void StartFight() {

            fightTeamEnemy.onAllInTeamDie.AddListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.AddListener(PlayerTeamDie);

            foreach (var charCtrl in fightTeamPlayer.chars) {
                charCtrl.GoAttack();
            }

            foreach (var charCtrl in fightTeamEnemy.chars) {
                charCtrl.GoAttack();
            }

            isInFight = true;

            // Refresh colors
            foreach (var cell in GameMng.current.cellsGridMng.currentRoom.GetCells()) {
                cell.ChangeColor();
            }

        }

        public TeamSide GetTeamSide(CharacterCtrl characterCtrl) {
            
            if (fightTeamEnemy.chars.Any(ctrl => ctrl == characterCtrl)) {
                return TeamSide.Enemy;
            }

            return TeamSide.Player;

        }

        private void EnemyTeamDie() {

            onPlayerWin.Invoke();

            fightTeamEnemy.onAllInTeamDie.RemoveListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.RemoveListener(PlayerTeamDie);

            isInFight = false;

        }

        private void PlayerTeamDie() {

            onEnemyTeamWin.Invoke();

            fightTeamEnemy.onAllInTeamDie.RemoveListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.RemoveListener(PlayerTeamDie);

            isInFight = false;

        }

    }
}
