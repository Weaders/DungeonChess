using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using UnityEngine;

namespace Assets.Scripts.Character {

    public class EffectsPlacer : MonoBehaviour {

        static int i = 0;

        public EffectOverridePlaceData[] overrieds;

        public Vector3 scaleMultiply = Vector3.one;

        /// <summary>
        /// Place effect
        /// Time in seconds
        /// </summary>
        /// <param name="effectPrefab"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public GameObject PlaceEffect(GameObject effectPrefab, float time = 0f) {

            var effectObj = Instantiate(effectPrefab, transform, false);

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

        public GameObject PlaceEffectWithoutTime(GameObject effectPrefab) {

            var effectObj = Instantiate(effectPrefab, transform, false);

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

    }
}
