using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using Assets.Scripts.Observable;
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

        public ObservableVal<bool> isInFight = new ObservableVal<bool>();

        public bool isThereSomeOneToFight => fightTeamPlayer.aliveChars.Any() && fightTeamEnemy.aliveChars.Any();

        public FightTeam GetEnemyTeamFor(CharacterCtrl characterCtrl) {

            if (fightTeamPlayer.characters.Contains(characterCtrl)) {
                return fightTeamEnemy;
            } else {
                return fightTeamPlayer;
            }

        }

        public FightTeam GetTeamFor(CharacterCtrl characterCtrl) {

            if (fightTeamPlayer.characters.Contains(characterCtrl)) {
                return fightTeamPlayer;
            } else {
                return fightTeamEnemy;
            }

        }

        public void MovePlayerCtrls() {

            var cells = GameMng.current.roomCtrl.currentRoom.GetCells().Where(c => 
                c.GetCellType() == CellsGrid.Cell.CellType.ForPlayer
            );

            foreach (var charCtrl in fightTeamPlayer.aliveChars) {

                var cellForMove = charCtrl.startCell == null ? charCtrl.cell : charCtrl.startCell;

                TagLogger<FightMng>.Info($"Start Search for character {charCtrl.name}, old position - {cellForMove.transform.position}");

                var closestCell = cells.MinElement(c => Vector3.Distance(c.transform.position, cellForMove.transform.position));

                TagLogger<FightMng>.Info($"Founded cell with position - {closestCell.transform.position}");

                closestCell.StayCtrl(charCtrl);

                cells = cells.Where(c => c != closestCell);

            }

        }

        public void StartFight() {

            GameMng.current.isBuildPhase.val = false;
            GameMng.current.buyPanelUI.selectedBuyData = null;

            fightTeamEnemy.onAllInTeamDie.AddListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.AddListener(PlayerTeamDie);

            foreach (var charCtrl in fightTeamPlayer.characters.Union(fightTeamEnemy.characters)) {
                charCtrl.characterData.ResetBeforeFight();
                charCtrl.GoAttack();
            }

            isInFight.val = true;

            // Refresh colors
            foreach (var cell in GameMng.current.roomCtrl.currentRoom.GetCells()) {
                cell.ChangeColor();
            }

            GameMng.current.topSidePanelUI.RefreshBtnInteractable();

        }

        public TeamSide GetTeamSide(CharacterCtrl characterCtrl) {

            if (fightTeamEnemy.characters.Any(ctrl => ctrl == characterCtrl)) {
                return TeamSide.Enemy;
            }

            return TeamSide.Player;

        }

        private void EnemyTeamDie() {

            onPlayerWin.Invoke();

            fightTeamEnemy.onAllInTeamDie.RemoveListener(EnemyTeamDie);

            StopAttack();

            isInFight.val = false;

            GameMng.current.topSidePanelUI.RefreshBtnInteractable();

        }

        private void PlayerTeamDie() {

            onEnemyTeamWin.Invoke();

            fightTeamEnemy.onAllInTeamDie.RemoveListener(EnemyTeamDie);
            fightTeamPlayer.onAllInTeamDie.RemoveListener(PlayerTeamDie);

            isInFight.val = false;

        }

        /// <summary>
        /// Stop attack for characters on battlefield
        /// </summary>
        private void StopAttack() {

            foreach (var character in fightTeamEnemy.characters.Union(fightTeamPlayer.characters)) {
                character.StopAttack();
            }

        }

    }
}
