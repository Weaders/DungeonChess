using UnityEngine;

namespace Assets.Scripts.AnimationCtrl {

    public class AnimEventForward : MonoBehaviour {

        public enum AnimEventType {
            AmimEventMakeAttack, EndSpellEvent
        }

        public ITargetForAnimEvents targetForAnimEvents { 
            get;
            set; 
        }

        /// <summary>
        /// Trigger anim event make
        /// </summary>
        /// <param name="scale"></param>
        public void AmimEventMakeAttack(float scale) {

            targetForAnimEvents.TriggerEvent(new AnimData {
                animEventType = AnimEventType.AmimEventMakeAttack,
                scale = scale,
                animEventForward = this
            });

        }

        /// <summary>
        /// On end animation
        /// </summary>
        public void OnEndAnimation() {

            targetForAnimEvents.TriggerEvent(new AnimData 
            {
                animEventType = AnimEventType.EndSpellEvent
            });

        }

        public class AnimData {

            public AnimEventType animEventType;

            public float scale;

            public AnimEventForward animEventForward;

        }

        public interface ITargetForAnimEvents {
            void TriggerEvent(AnimData animData);
        }

    }
}
