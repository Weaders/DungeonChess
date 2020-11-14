using Assets.Scripts.Character;
using Assets.Scripts.Spells;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.AnimationCtrl.AnimEventForward;

namespace Assets.Scripts.AnimationCtrl {
    public abstract class SpellAnimationData : MonoBehaviour {

        public abstract AnimEventData RunFor(Spell spell, CharacterCtrl from, CharacterCtrl to);

        public class AnimEventData : UnityEvent<AnimData> {
        }
    }
}
