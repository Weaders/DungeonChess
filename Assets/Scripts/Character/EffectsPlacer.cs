using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using UnityEngine;

namespace Assets.Scripts.Character {

    public class EffectsPlacer : MonoBehaviour {

        static int i = 0;

        public EffectOverridePlaceData[] overrieds;

        public GameObject PlaceEffect(GameObject effectPrefab) {

            var effectObj = Instantiate(effectPrefab, transform, false);

            effectObj.name = $"Effect_{i++}";

            ProcessEffect(effectObj);

            return effectObj;

        }

        public void ProcessEffect(GameObject effectObj) {

            var effect = effectObj.GetComponent<EffectObj>();

            var overrideData = overrieds.FirstOrDefault(e => e.effectId == effect.id);

            if (overrideData == null) {

                effectObj.transform.localPosition
                   = Vector3.zero + effect.offset;

                effectObj.transform.localScale
                    = Vector3.one + effect.offsetScale;

            } else {

                effectObj.transform.localPosition
                   = Vector3.zero + overrideData.offsetPosition;

                effectObj.transform.localScale
                    = Vector3.one + overrideData.offsetScale;

            }
        }

        [ContextMenu("PlaceExistsEffects")]
        public void PlaceEffects() {

            foreach (Transform obj in transform)
                if (obj.tag == TagsStore.EFFECT)
                    ProcessEffect(obj.gameObject);

        }

        [Serializable]
        public class EffectOverridePlaceData {

            public string effectId;

            public Vector3 offsetPosition;

            public Vector3 offsetScale;


        }

    }
}
