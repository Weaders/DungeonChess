using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.CellsGrid;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    public class SimulateCharacterCtrl : CharacterCtrl {

        public CharacterCtrl characterToSimulate;

        public CharacterCtrl ctrlToAttack;

        public Queue<Cell> path { get; set; } = new Queue<Cell>();

        public bool isMoveToTarget => characterActionsCtrl.currentPath == null || characterActionsCtrl.currentPath.isMovedToTarget;

        public override IEnumerator AttackWhileInRange(CharacterCtrl target, UnityAction onEnd) {

            ctrlToAttack = target;
            return null;

        }

        private void Awake() {
            characterActionsCtrl = new CharacterActionsCtrl(this);
        }

        private void Update() {

        }

        public override bool IsTargetableFor(CharacterCtrl characterCtrl) {
            return base.IsTargetableFor(characterCtrl) && characterToSimulate != characterCtrl && characterData.cell != null;
        }

        public IEnumerator Move() {

            if (characterData.cell != null) {

                characterActionsCtrl.ClearCellToMove();

                var actions = characterActionsCtrl.IterateAction(true);

                while (actions != null) {

                    while (actions.MoveNext()) {
                        yield return actions.Current;
                    }

                    actions = characterActionsCtrl.IterateAction(true);

                }

                DrawMovePath();

            }

        }
        
        private void DrawMovePath() {

            if (characterActionsCtrl.cellsToMove.Any()) {

                var path = Instantiate(GameMng.current.simulateCharacterMoveCtrl.lineRendererPrefab.gameObject, transform);

                var lineRenderer = path.GetComponent<LineRenderer>();

                lineRenderer.material = GameMng.current.simulateCharacterMoveCtrl.move;
                lineRenderer.material.SetColor("_BaseColor", Color.green);

                var startCell = characterToSimulate.characterData.cell;

                var points = characterActionsCtrl.cellsToMove.Prepend(startCell).Select(c => c.transform.position + c.transform.up).ToArray();

                lineRenderer.positionCount = points.Length;
                lineRenderer.SetPositions(points);

                if (ctrlToAttack != null) {

                    var attackPath = Instantiate(GameMng.current.simulateCharacterMoveCtrl.lineRendererPrefab.gameObject, transform);

                    var from = lineRenderer.GetPosition(lineRenderer.positionCount - 1);

                    var to = ctrlToAttack.characterData.cell.transform.position + ctrlToAttack.characterData.cell.transform.up;

                    var attackLineRenderer = attackPath.GetComponent<LineRenderer>();

                    attackLineRenderer.material = GameMng.current.simulateCharacterMoveCtrl.move;
                    attackLineRenderer.material.SetColor("_BaseColor", Color.red);

                    attackLineRenderer.positionCount = 2;
                    attackLineRenderer.SetPositions(new[] { from, to });

                }

            }

        }

        public static SimulateCharacterCtrl CreateFromCharacterCtrl(CharacterCtrl characterCtrl) {

            var obj = new GameObject("Simulate", typeof(SimulateCharacterCtrl), typeof(CharacterMoveCtrl));

            obj.transform.SetParent(characterCtrl.transform);

            var characterData = Instantiate(characterCtrl.characterData, obj.transform);

            var simCharacterCtrl = obj.GetComponent<SimulateCharacterCtrl>();
            var moveCtrl = obj.GetComponent<CharacterMoveCtrl>();

            moveCtrl.characterCtrl = simCharacterCtrl;

            simCharacterCtrl.characterData = characterData;
            simCharacterCtrl.moveCtrl = characterCtrl.moveCtrl;
            simCharacterCtrl.characterData.Init();
            simCharacterCtrl.characterToSimulate = characterCtrl;
            simCharacterCtrl.moveCtrl = moveCtrl;

            return simCharacterCtrl;

        }

    }

}
