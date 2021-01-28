using System.Collections;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Effects {

    public class EffectObj : MonoBehaviour {

        public string id;

        public Vector3 offset;

        public Vector3 offsetScale;

        public float defaultDuration = 1f;

        public UnityEvent<CharacterCtrl> onTouch = new UnityEvent<CharacterCtrl>();

        public UnityEvent onCome = new UnityEvent();

        public UnityEvent onStart = new UnityEvent();

        public void MoveToTransorm(Transform target, float speed)
            => StartCoroutine(MoveToTrasformCoroutine(target, speed));

        private IEnumerator MoveToTrasformCoroutine(Transform tr, float speed) {

            onStart.Invoke();

            while (Vector3.Distance(tr.position, transform.position) > 0.1f) {

                transform.position = Vector3.MoveTowards(transform.position, tr.position, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();

            }

            onCome.Invoke();

        }

        public void MoveToDirection(Vector3 position, float speed, float time)
            => StartCoroutine(MoveToDirectionCoroutine(position, speed, time));

        private IEnumerator MoveToDirectionCoroutine(Vector3 direciton, float speed, float time) {

            onStart.Invoke();

            var dir = (direciton - transform.position).normalized * speed;

            while (time > 0) {

                transform.position += dir * Time.deltaTime;
                time -= Time.deltaTime;
                yield return null;

            }

            onCome.Invoke();

        }

        public void StayOnCharacterCtrl(CharacterCtrl characterCtrl, float? time = null)
            => StartCoroutine(StayOnCharacterCtrlCroutine(characterCtrl, time));

        private IEnumerator StayOnCharacterCtrlCroutine(CharacterCtrl characterCtrl, float? time = null) {
            
            onStart.Invoke();

            if (time == null)
                time = defaultDuration;

            while (time > 0) {

                characterCtrl.effectsPlacer.PlaceEffect(gameObject);
                time -= Time.deltaTime;
                yield return null;

            }

            onCome.Invoke();
        }

        public void OnParticleCollision(GameObject other) {

            var ctrl = other.GetComponent<CharacterCtrl>();

            if (ctrl != null)
                onTouch.Invoke(ctrl);

        }

    }

}
