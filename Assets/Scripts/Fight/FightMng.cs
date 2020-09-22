using System.Linq;
using Assets.Scripts.Character;
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

        public FightTeam GetEnemyTeamFor(CharacterCtrl characterCtrl) {

            if (fightTeamPlayer.chars.Contains(characterCtrl)) {
                return fightTeamEnemy;
            } else {
                return fightTeamPlayer;
            }

        }

        public void RefreshEnemies() {

            fightTeamEnemy.ClearChars();

            var enumerator = GameMng.current.cellsGridMng.enemiesSideCell.GetEnumerator();

            var i = 0;

            foreach (var enemyData in GameMng.current.currentDungeonData.enemiesPoll.enemies) {

                enumerator.MoveNext();
                var ctrlObj = enumerator.Current.StayCtrlPrefab(enemyData.characterCtrl);

                ctrlObj.gameObject.name = $"EnemyCtrl_{i}";

                fightTeamEnemy.AddCharacterToTeam(ctrlObj);
                GameMng.current.dropCtrl.AddDrop(ctrlObj, enemyData.sortedDropChanges);

                ++i;
            }

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

        }

        private void PlayerTeamDie() {

            onEnemyTeamWin.Invoke();

            fightTeamEnemy.onAllInTeamDie.RemoveListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.RemoveListener(PlayerTeamDie);

        }

    }
}
