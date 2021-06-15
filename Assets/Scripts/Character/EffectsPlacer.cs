using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using UnityEngine;
using static Assets.Scripts.Effects.EffectObj;

namespace Assets.Scripts.Character {

    public class EffectsPlacer : MonoBehaviour {

        static int i = 0;

        public EffectOverridePlaceData[] overrieds;

        public Vector3 scaleMultiply = Vector3.one;

        [SerializeField]
        private TargetObj[] targetObjs;

        [SerializeField]
        private CharacterCtrl characterCtrl;

        private void Awake() {
            characterCtrl = GetComponent<CharacterCtrl>();
        }

        /// <summary>
        /// Place effect
        /// Time in seconds
        /// </summary>
        /// <param name="effectPrefab"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public GameObject PlaceEffect(GameObject effectPrefab, float time = 0f, BindTarget bindTarget = BindTarget.Default) {

            var effectObj = Instantiate(effectPrefab, GetTarget(bindTarget, true), false);

            effectObj.name = $"Effect_{i++}";

            var effect = effectObj.GetComponent<EffectObj>();

            ProcessEffect(effect);

            if (time > -1) {

                if (time == default)
                    Destroy(effectObj, effect.defaultDuration);
                else
                    Destroy(effectObj, time);
            }


            return effectObj;

        }

        public GameObject PlaceEffectWithoutTime(GameObject effectPrefab, BindTarget bindTarget = BindTarget.Default) {

            var effectObj = Instantiate(effectPrefab, GetTarget(bindTarget, true), false);

            effectObj.transform.rotation.Set(0, 0, 0, 1);

            effectObj.name = $"Effect_{i++}";

            var effect = effectObj.GetComponent<EffectObj>();

            ProcessEffect(effect);

            return effectObj;

        }

        public void ProcessEffect(EffectObj effect, float time = 0f) {

            var overrideData = overrieds.FirstOrDefault(e => e.effectId == effect.id);

            if (overrideData == null) {

                effect.transform.localPosition
                   = Vector3.zero + effect.offset;

                effect.transform.localScale
                    = Vector3.Scale(effect.transform.localScale, scaleMultiply);

            } else {

                effect.transform.localPosition
                   = Vector3.zero + overrideData.offsetPosition;

                effect.transform.localScale
                    = Vector3.one + overrideData.offsetScale;

            }

        }

        public void PlaceEffect(Effect effect, BindTarget target = BindTarget.Default) {
            
            effect.transform.SetParent(GetTarget(target, true), false);
            effect.transform.rotation.Set(0, 0, 0, 1);
            effect.name = $"Effect_{i++}";

        }

        public Transform GetTarget(BindTarget bindTarget, bool forStay = false) {

            switch (bindTarget) {

                case BindTarget.Head when characterCtrl.headTransform != null:
                    return characterCtrl.headTransform;
                case BindTarget.Default when forStay:
                    return characterCtrl.transform;
                case BindTarget.Default when !forStay:
                default:
                    return characterCtrl.GetTargetTransform();

            }

        }

        [ContextMenu("PlaceExistsEffects")]
        public void PlaceEffects() {

            foreach (Transform obj in transform)
                if (obj.tag == TagsStore.EFFECT)
                    ProcessEffect(obj.gameObject.GetComponent<EffectObj>());

        }

        [Serializable]
        public class EffectOverridePlaceData {

            public string effectId;

            public Vector3 offsetPosition;

            public Vector3 offsetScale;

        }

        [Serializable]
        public class TargetObj {

            [SerializeField]
            private BindTarget bindTarget;
            [SerializeField]
            private Transform obj;

        }

    }
}
