using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Effects {
    public class Effect : MonoBehaviour {

        [SerializeField]
        private string _id;

        public string id => _id;

        /// <summary>
        /// Remove object on come
        /// Uses only when <see cref="EffectAction.Move"/>
        /// </summary>
        public bool onComeDestroy;

        public BindTarget bindTarget;

        public EffectAction effectAction { get; private set; }

        public CharacterCtrl target { get; private set; }

        public float moveSpeed { get; set; }

        /// <summary>
        /// Current move result
        /// </summary>
        public MoveResult moveResult { get; private set; }

        /// <summary>
        /// Invoked on start effect, after call <see cref="MoveToCharacter(CharacterCtrl, EffectObj.BindTarget)"/> or <see cref="PlaceForCharacter(CharacterCtrl, EffectObj.BindTarget)"/>
        /// </summary>
        public UnityEvent onStart { get; private set; } = new UnityEvent();

        [SerializeField]
        private ParticleEffectRunner particleEffectRunner;

        public void PlaceForCharacter(CharacterCtrl ctrl) {

            target = ctrl;

            effectAction = EffectAction.Place;

            ctrl.effectsPlacer.PlaceEffect(this);

            Play();

        }

        public void MoveToCharacter(CharacterCtrl ctrl, BindTarget bind = BindTarget.Default) {

            effectAction = EffectAction.Place;
            bindTarget = bind;
            target = ctrl;

        }

        public void Play() {

            effectAction = EffectAction.Place;

            if (particleEffectRunner != null) {
                particleEffectRunner.Run();
            }

        }

        public void Stop() {

            effectAction = EffectAction.None;
            
            if (particleEffectRunner != null) {
                particleEffectRunner.Stop();
            }

        }

        private void Update() {

            if (effectAction == EffectAction.Move) {

                var bindTargetObj = target.effectsPlacer.GetTarget(bindTarget);

                if (Vector3.Distance(transform.position, bindTargetObj.position) > 0.01f) {
                    transform.Translate((bindTargetObj.position - transform.position).normalized * Time.deltaTime * moveSpeed);
                } else {

                    if (!moveResult.isCome) {

                        moveResult.onTouch.Invoke(target);
                        moveResult.onCome.Invoke(this);

                    }

                    moveResult.isCome = true;

                    if (onComeDestroy)
                        Destroy(gameObject);

                }

            } 

        }

        public class MoveResult {

            /// <summary>
            /// When touch character
            /// </summary>
            public UnityEvent<CharacterCtrl> onTouch = new UnityEvent<CharacterCtrl>();

            /// <summary>
            /// When come to character
            /// </summary>
            public UnityEvent<Effect> onCome = new UnityEvent<Effect>();

            public bool isCome { get; set; }

        }

        public enum EffectAction {
            None, Place, Move
        }

    }
}
