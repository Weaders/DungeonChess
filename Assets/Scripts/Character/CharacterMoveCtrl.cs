using System.Collections;
using System.Linq;
using Assets.Scripts.CellsGrid;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    public class CharacterMoveCtrl : MonoBehaviour {

        [SerializeField]
        private CharacterCtrl ctrl;

        [SerializeField]
        private NavMeshAgent navMeshAgent;

        public NavMeshAgent GetNavMeshAgent() => navMeshAgent;

        public MoveData MoveToCharacter(CharacterCtrl characterCtrl) {

            UnityEvent onCome = new UnityEvent();

            var result = new MoveData(() => {

                var path = GameMng.current.pathToCell.GetPath(ctrl.cell, characterCtrl.cell);

                ctrl.anim.SetBool("IsWalk", true);

                var enumerator = path.GetToMovePath().GetEnumerator();

                var currentTargetCell = characterCtrl.cell;

                void toNextCell() {

                    if (currentTargetCell != characterCtrl.cell) {
                        enumerator = GameMng.current.pathToCell.GetPath(ctrl.cell, characterCtrl.cell).GetToMovePath().GetEnumerator();
                    }

                    if (enumerator.MoveNext()) {
                        StartCoroutine(MoveToCell(enumerator.Current, toNextCell));
                    } else {

                        ctrl.anim.SetBool("IsWalk", false);
                        onCome.Invoke();

                    }

                }

                toNextCell();

            }) {
                onCome = onCome
            };

            return result;
        }


        #region Use without path, not used currently

        public MoveData MoveTo(Transform transform, float distance) {

            TargetForMove targetForMove = new TargetForMove() {
                target = transform,
                distance = distance
            };

            var result = new MoveData(() => StartCoroutine(MoveToTarget(targetForMove)));

            targetForMove.onCome = result.onCome;

            return result;

        }

        private void Start() {
            //WTF!Unity!WTF!
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
            navMeshAgent.updateRotation = false;
        }

        private IEnumerator MoveToCell(Cell cell, UnityAction onCome) {

            cell.StayCtrl(ctrl, false);

            var t = 0f;

            var startPosition = ctrl.transform.position;

            while (t < 1f) {

                t += Time.deltaTime * ctrl.characterData.stats.moveSpeed / 5f;

                var newPos = Vector3.Lerp(startPosition, cell.transform.position, t);

                ctrl.transform.LookAt(new Vector3(
                    cell.transform.position.x,
                    ctrl.transform.position.y,
                    cell.transform.position.z
                ));

                ctrl.transform.position = new Vector3(newPos.x, ctrl.transform.position.y, newPos.z);

                yield return new WaitForFixedUpdate();

            }

            onCome.Invoke();

        }

        private IEnumerator MoveToTarget(TargetForMove targetForMove, bool isDisableWalk = true) {

            ctrl.anim.SetBool("IsWalk", true);

            //navMeshAgent.isStopped = false;
            var t = 0f;

            while (targetForMove.target != null && Vector3.Distance(ctrl.transform.position, targetForMove.target.position) > targetForMove.distance) {

                t += Time.deltaTime * ctrl.characterData.stats.moveSpeed / 100;

                var newPos = Vector3.Lerp(ctrl.transform.position, targetForMove.target.position, t);
                
                ctrl.transform.LookAt(targetForMove.target.transform);
                ctrl.transform.position = newPos;

                yield return new WaitForFixedUpdate();

            }

            //navMeshAgent.isStopped = true;

            if (isDisableWalk)
                ctrl.anim.SetBool("IsWalk", false);

            targetForMove.onCome.Invoke();

        }

        public void DisableNavMesh() {
            navMeshAgent.enabled = false;
        }

        public void EnableNavMesh() {
            navMeshAgent.enabled = true;
        }

        #endregion

        public class TargetForMove {

            public Transform target;
            public float distance;
            public UnityEvent onCome;

        }

        public class MoveData {

            private readonly UnityAction _onStart;

            public MoveData(UnityAction onStart) {
                _onStart = onStart;
            }

            public UnityEvent onCome = new UnityEvent();

            public void Start() => _onStart.Invoke();

        }

    }
}
