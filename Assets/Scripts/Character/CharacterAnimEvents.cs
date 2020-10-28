using UnityEngine;

namespace Assets.Scripts.Character {

    public class CharacterAnimEvents : MonoBehaviour {

        private CharacterCtrl characterCtrl;

        public void Init(CharacterCtrl ctrl) {
            characterCtrl = ctrl;
        }

        /// <summary>
        /// Called by animation
        /// </summary>
        /// <param name="scale"></param>
        public void AmimEventMakeAttack(float scale) {

            if (characterCtrl.targetForAttack != null)
                characterCtrl.MakeBaseAttack(characterCtrl.targetForAttack, scale);

        }

    }

}
