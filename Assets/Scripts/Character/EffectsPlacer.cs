using System;
using System.Linq;
using Assets.Scripts.Effects;
using UnityEngine;

namespace Assets.Scripts.Character {

    public class EffectsPlacer : MonoBehaviour {

        public Vector3 offsetPosition;

        public Vector3 offsetScale;

        public EffectOverridePlaceData[] overrieds;

        public GameObject PlaceEffect(GameObject effectPrefab) {

            var effectObj = Instantiate(effectPrefab, transform);

            ProcessEffect(effectObj);

            return effectObj;

        }

        public void ProcessEffect(GameObject effectObj) {

            var effect = effectObj.GetComponent<EffectObj>();

            var overrideData = overrieds.FirstOrDefault(e => e.effectId == effect.id);

            if (overrideData == null) {

                effectObj.transform.localPosition
                   = effectObj.transform.localPosition + offsetPosition;

                effectObj.transform.localScale
                    = effectObj.transform.localScale + offsetScale;

            } else {

                effectObj.transform.localPosition
                   = effectObj.transform.localPosition + overrideData.offsetPosition;

                effectObj.transform.localScale
                    = effectObj.transform.localScale + overrideData.offsetScale;

            }
        }

        [ContextMenu("PlaceExistsEffects")]
        public void PlaceEffects() {

            foreach (Transform obj in transform)
                if (obj.tag == "Effect")
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
