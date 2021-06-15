using Assets.Scripts.AnimationCtrl;
using UnityEngine;

namespace Assets.Scripts.Character {

    /// <summary>
    /// Used for combine animation of character and spell animations
    /// </summary>
    public class CharacterAnimEvents : MonoBehaviour {

        private CharacterCtrl characterCtrl;

        public void Init(CharacterCtrl ctrl) {
            characterCtrl = ctrl;
        }

        /// <summary>
        /// Called by animation, for make attack
        /// </summary>
        /// <param name="scale"></param>
        public void AmimEventMakeAttack(AnimAttackData animAttackData) {

            if (animAttackData == null)
                animAttackData = AnimAttackData.GetNew();

            if (characterCtrl.targetForAttack != null) {

                if (characterCtrl.isCanMakeFullManaAttack) {
                    characterCtrl.MakeFullManaAttack(characterCtrl.targetForAttack, animAttackData);
                } else {
                    characterCtrl.MakeBaseAttack(characterCtrl.targetForAttack, animAttackData);
                }

            }
                

        }

        public void StartAnimEventMakeAttack() {

            if (characterCtrl.targetForAttack != null) {
                characterCtrl.StartMakeAttack(characterCtrl.isCanMakeFullManaAttack);
            }

        }

    }

}
