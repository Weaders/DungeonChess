using System;
using Assets.Scripts.Character;
using Assets.Scripts.Spells;
using UnityEngine;
using UnityEngine.Events;
using static Assets.Scripts.AnimationCtrl.AnimEventForward;
using static Assets.Scripts.Spells.Spell;

namespace Assets.Scripts.AnimationCtrl {

    public abstract class SpellAnimationData : MonoBehaviour {

        public abstract AnimRunResult RunFor(Spell spell, CharacterCtrl from, CharacterCtrl to, UseSpellOpts opts);

        public class AnimRunResult {

            public Func<bool> IsEndAnim;
            public AnimEventData animEventData;

        }

        public class AnimEventData : UnityEvent<AnimData> {
        }
    }
}
