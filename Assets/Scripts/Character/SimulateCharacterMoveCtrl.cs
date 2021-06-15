using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Fight;
using UnityEngine;

namespace Assets.Scripts.Character {

    public class SimulateCharacterMoveCtrl : MonoBehaviour {

        public LineRenderer lineRendererPrefab;

        public Material move;
        public Material attack;

        #region SimulateTeam 
        public FightTeam simulateTeamPlayer { get; private set; } = new FightTeam(TeamSide.Player, true);
        public FightTeam simulateTeamEnemy { get; private set; } = new FightTeam(TeamSide.Enemy, true);
        #endregion

        [ContextMenu("Simulate")]
        public void Simulate() {

            return;

            RemoveCharacters();

            var playerTeam = GameMng.current.fightMng.fightTeamPlayer;
            var enemyTeam = GameMng.current.fightMng.fightTeamEnemy;

            if (!playerTeam.aliveChars.Any() || !enemyTeam.aliveChars.Any())
                return;

            var characters = new List<CharacterActionData>(playerTeam.aliveChars.Count() + enemyTeam.aliveChars.Count());

            foreach (var character in GameMng.current.fightMng.GetCharacters()) {

                var simulateCharacterCtrl = SimulateCharacterCtrl.CreateFromCharacterCtrl(character);

                if (character.teamSide == TeamSide.Enemy) {
                    simulateTeamEnemy.AddCharacterToTeam(simulateCharacterCtrl);
                } else {
                    simulateTeamPlayer.AddCharacterToTeam(simulateCharacterCtrl);
                }

                var iterator = simulateCharacterCtrl.Move();

                characters.Add(new CharacterActionData 
                {
                    ctrl = simulateCharacterCtrl,
                    actions = iterator
                });
                
            }

            int completed = 0;

            var ctrlsWithoutMoveToTarget = new HashSet<SimulateCharacterCtrl>();

            while (completed < characters.Count) {

                foreach (var character in characters) {

                    if (character.actions != null && !character.completed) {

                        if (!character.ctrl.isMoveToTarget)
                            ctrlsWithoutMoveToTarget.Add(character.ctrl);
                        else
                            ctrlsWithoutMoveToTarget.Remove(character.ctrl);

                        if (ctrlsWithoutMoveToTarget.Count == characters.Count - completed) {

                            character.completed = true;
                            completed++;

                            ctrlsWithoutMoveToTarget.Remove(character.ctrl);

                        } else {

                            character.completed = !character.actions.MoveNext();

                            if (character.completed)
                                completed++;

                        }

                        

                    }

                }

            }

        }

        public FightTeam GetEnemyTeamFor(CharacterCtrl characterCtrl) {

            if (simulateTeamPlayer.characters.Any(c => c.characterData == characterCtrl.characterData)) {
                return simulateTeamEnemy;
            } else {
                return simulateTeamPlayer;
            }

        }

        public void RemoveCharacters() {

            simulateTeamPlayer.ClearChars();
            simulateTeamEnemy.ClearChars();

        }

        public class CharacterActionData { 
        
            public SimulateCharacterCtrl ctrl { get; set; }
            public IEnumerator actions { get; set; }
            public bool completed { get; set; }

        }
    }
}
