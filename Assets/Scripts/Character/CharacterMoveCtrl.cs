﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Assets.Scripts.Character {

    public class CharacterMoveCtrl : MonoBehaviour {

        [SerializeField]
        private CharacterCtrl ctrl;

        [SerializeField]
        private NavMeshAgent navMeshAgent;

        public MoveData MoveTo(Transform transform, float distance) {

            TargetForMove targetForMove = null;

            var result = new MoveData(() => StartCoroutine(MoveToTarget(targetForMove)));

            targetForMove = new TargetForMove() {
                target = transform,
                distance = distance,
                onCome = result.onCome
            };

            return result;

        }

        private void Start() {
            //WTF!Unity!WTF!
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
            navMeshAgent.updateRotation = false;
        }

        private IEnumerator MoveToTarget(TargetForMove targetForMove) {

            ctrl.anim.SetBool("IsWalk", true);

            while (Vector3.Distance(ctrl.transform.position, targetForMove.target.position) > targetForMove.distance) {

                navMeshAgent.SetDestination(targetForMove.target.position);

                ctrl.transform.LookAt(targetForMove.target.transform);

                //GameLogger<CharacterMoveCtrl>.InfoMany(
                //  $"Move towards for speed - {ctrl.characterData.stats.moveSpeed}, {Time.deltaTime}",
                //  Vector3.Distance(ctrl.transform.position, targetForMove.target.position),
                //  $"D: {targetForMove.distance}"
                //);

                yield return new WaitForFixedUpdate();
            }

            navMeshAgent.isStopped = true;

            ctrl.anim.SetBool("IsWalk", false);

            targetForMove.onCome.Invoke();

        }

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