using System.Collections;
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
        /// Remove before use ult
        /// </summary>
        public bool removeOnPreUseUtl;

        /// <summary>
        /// Remove object on come
        /// Uses only when <see cref="EffectAction.Move"/>
        /// </summary>
        public bool onComeDestroy;

        public EffectAction effectAction { get; private set; }

        public CharacterCtrl target { get; private set; }

        public EffectObj.BindTarget bindTarget { get; private set; }

        public float moveSpeed { get; set; }

        /// <summary>
        /// Current move result
        /// </summary>
        public MoveResult moveResult { get; private set; }

        /// <summary>
        /// Invoked on start effect, after call <see cref="MoveToCharacter(CharacterCtrl, EffectObj.BindTarget)"/> or <see cref="PlaceForCharacter(CharacterCtrl, EffectObj.BindTarget)"/>
        /// </summary>
        public UnityEvent onStart { get; private set; } = new UnityEvent();
        
        public void PlaceForCharacter(CharacterCtrl ctrl, EffectObj.BindTarget bind = EffectObj.BindTarget.Default) {

            target = ctrl;
            
            effectAction = EffectAction.Place;

            bindTarget = bind;
            
            ctrl.effectsPlacer.PlaceEffect(this, bindTarget);

            if (removeOnPreUseUtl) {

                void Remove() {
                    Destroy(gameObject);
                    ctrl.characterData.onPreUseUlt.RemoveSubscription(Remove);
                }

                ctrl.characterData.onPreUseUlt.AddSubscription(OrderVal.Internal, Remove);
            }

        }

        public void MoveToCharacter(CharacterCtrl ctrl, EffectObj.BindTarget bind = EffectObj.BindTarget.Default) {

            effectAction = EffectAction.Place;
            bindTarget = bind;
            target = ctrl;

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

            } else {
                target.effectsPlacer.PlaceEffect(this, bindTarget);
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
            Place, Move
        }

    }
}
