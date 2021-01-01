using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.State {

    public abstract class StateData : MonoBehaviour {

        public abstract Sprite icon { get; }

        public abstract StateType stateType { get; }

        protected CharacterCtrl owner { get; private set; }

        public void Apply(CharacterCtrl ctrl) {
            owner = ctrl;
            OnApply();
        }

        public void DeApply() {
            owner = null;
            OnDeApply();
        }

        protected abstract void OnApply();

        protected abstract void OnDeApply();

        public enum StateType { 
            Stun, NoMove
        }

        public bool isCharCanMove => stateType != StateType.Stun;

        public bool isCharCanAttack => stateType != StateType.Stun;

    }

}
