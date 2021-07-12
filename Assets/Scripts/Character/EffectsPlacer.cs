using System;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using UnityEngine;

namespace Assets.Scripts.Character {

    public enum BindTarget {
        Default, Head, Rig
    }

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
        /// Place effecf from prefab.
        /// For already exists effect use <see cref="PlaceEffect(Effect)"/>
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public GameObject PlaceEffectPrefab(Effect effect) {

            var prefab = Instantiate(effect.gameObject);

            var effectObj = prefab.GetComponent<Effect>();

            PlaceEffect(effectObj);
            effectObj.Play();

            return prefab;

        }


        public void PlaceEffect(Effect effect) {
            
            effect.transform.SetParent(GetTarget(effect.bindTarget, true), true);

            effect.transform.rotation.Set(0, 0, 0, 1);

            effect.transform.localPosition = Vector3.zero;
            effect.transform.localScale = Vector3.Scale(effect.transform.localScale, scaleMultiply);

            effect.name = $"Effect_{i++}";

        }

        public Transform GetTarget(BindTarget bindTarget, bool forStay = false) {

            switch (bindTarget) {

                case BindTarget.Head when characterCtrl.headTransform != null:
                    return characterCtrl.headTransform;
                case BindTarget.Rig when targetObjs.Any(t => t.bind == BindTarget.Rig):
                    return targetObjs.First(t => t.bind == BindTarget.Rig).transform;
                case BindTarget.Default when forStay:
                    return characterCtrl.transform;
                case BindTarget.Default when !forStay:
                default:
                    return characterCtrl.GetTargetTransform();

            }

        }

        [ContextMenu("PlaceExistsEffects")]
        public void PlaceEffects() {

            foreach (var effect in GetComponentsInChildren<Effect>()) {

                if (effect.gameObject.tag == TagsStore.EFFECT)
                    PlaceEffect(effect);

            }                    

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

            public BindTarget bind => bindTarget;

            public Transform transform => obj;

        }

    }
}
