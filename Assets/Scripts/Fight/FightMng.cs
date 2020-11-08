using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
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

        public void MovePlayerCtrls() {

            var cells = GameMng.current.roomCtrl.currentRoom.GetCells();

            foreach (var charCtrl in fightTeamPlayer.aliveChars) {

                TagLogger<FightMng>.Info($@"Start Search for character {charCtrl.name}, old position - {charCtrl.cell.transform.position}");

                var closestCell = cells.MinElement(c => Vector3.Distance(c.transform.position, charCtrl.cell.transform.position));

                TagLogger<FightMng>.Info($"Founded cell with position - {closestCell.transform.position}");

                closestCell.StayCtrl(charCtrl);

            }

        }

        public void StartFight() {

            GameMng.current.buyPanelUI.selectedBuyData = null;

            fightTeamEnemy.onAllInTeamDie.AddListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.AddListener(PlayerTeamDie);

            foreach (var charCtrl in fightTeamPlayer.chars) {
                charCtrl.characterData.ResetBeforeFight();
                charCtrl.GoAttack();
            }

            foreach (var charCtrl in fightTeamEnemy.chars) {
                charCtrl.characterData.ResetBeforeFight();
                charCtrl.GoAttack();
            }

            isInFight = true;

            // Refresh colors
            foreach (var cell in GameMng.current.roomCtrl.currentRoom.GetCells()) {
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
