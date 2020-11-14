using UnityEngine;

namespace Assets.Scripts.AnimationCtrl {

    public class AnimEventForward : MonoBehaviour {

        public enum AnimEventType {
            AmimEventMakeAttack, EndSpellEvent
        }

        public ITargetForAnimEvents targetForAnimEvents;

        /// <summary>
        /// Trigger from 
        /// </summary>
        /// <param name="scale"></param>
        public void AmimEventMakeAttack(float scale) {

            targetForAnimEvents.TriggerEvent(new AnimData {
                animEventType = AnimEventType.AmimEventMakeAttack,
                scale = scale,
                animEventForward = this
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
