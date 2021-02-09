using System.Collections;
using Assets.Scripts.AnimationCtrl;
using Assets.Scripts.Character;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.AnimationCtrl.AnimEventForward;

namespace Assets.Scripts.Effects {

    public class EffectObj : MonoBehaviour, ITargetForAnimEvents {

        public string id;

        public Vector3 offset;

        public float defaultDuration = 1f;

        public UnityEvent<CharacterCtrl> onTouch = new UnityEvent<CharacterCtrl>();

        public UnityEvent onCome = new UnityEvent();

        public UnityEvent onStart = new UnityEvent();

        private UnityEvent onEndAnimation = new UnityEvent();

        private CharacterCtrl _currentTarget = null;

        public UnityEvent onDestroy { get; private set; } = new UnityEvent();

        [SerializeField]
        private EffectObj exposionPrefab;

        private void Start() {

            var forwardEvents = GetComponent<AnimEventForward>();

            if (forwardEvents != null)
                forwardEvents.targetForAnimEvents = this;

        }

        public void MoveToTransorm(Transform target, float speed)
            => StartCoroutine(MoveToTrasformCoroutine(target, speed));

        private IEnumerator MoveToTrasformCoroutine(Transform tr, float speed) {

            onStart.Invoke();

            while (Vector3.Distance(tr.position, transform.position) > 0.1f) {

                transform.position = Vector3.MoveTowards(transform.position, tr.position, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();

            }

            if (exposionPrefab != null) {

                var explosionObj = Instantiate(exposionPrefab);
                explosionObj.transform.position = transform.position;

                Destroy(explosionObj.gameObject, explosionObj.defaultDuration);
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

        public void StayOnCharacterCtrl(CharacterCtrl characterCtrl, float? time = null, bool waitForEndAnimation = false)
            => StartCoroutine(StayOnCharacterCtrlCroutine(characterCtrl, time, waitForEndAnimation));

        private IEnumerator StayOnCharacterCtrlCroutine(CharacterCtrl characterCtrl, float? time = null, bool waitForEndAnimation = false) {

            _currentTarget = characterCtrl;

            onEndAnimation.RemoveAllListeners();
            onStart.Invoke();

            if (time == null)
                time = defaultDuration;

            characterCtrl.effectsPlacer.PlaceEffect(gameObject, time.Value);

            if (waitForEndAnimation) {

                var animIsEnd = false;

                onEndAnimation.AddListener(() => animIsEnd = true);

                yield return new WaitUntil(() => animIsEnd);

            } else {

                yield return new WaitForSeconds(time.Value);

            }            

            onCome.Invoke();

        }

        public void OnParticleCollision(GameObject other) {

            var ctrl = other.GetComponent<CharacterCtrl>();

            if (ctrl != null)
                onTouch.Invoke(ctrl);

        }

        public void TriggerEvent(AnimData animData) {

            if (animData.animEventType == AnimEventType.EndSpellEvent)
                onEndAnimation.Invoke();
            else
                onTouch.Invoke(_currentTarget);

        }

        private void OnDestroy() {
            onDestroy.Invoke();
        }

    }

}
