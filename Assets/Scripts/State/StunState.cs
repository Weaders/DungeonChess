using System.Collections;
using UnityEngine;

namespace Assets.Scripts.State {
    public class StunState : StateData {

        public override Sprite icon => GameMng.current.gameData.staticStatesData.stunImg;

        public float secondsTimeout;

        public override StateType stateType => StateType.Stun;

        protected override void OnApply() {
            StartCoroutine(RemoveBuff());
        }

        protected override void OnDeApply() { }

        private IEnumerator RemoveBuff() {
            yield return new WaitForSeconds(secondsTimeout);
            owner.characterData.stateContainer.Remove(this);
        }

    }
}
