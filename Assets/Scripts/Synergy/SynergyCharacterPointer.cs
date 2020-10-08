using System.Collections.Generic;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Synergy {

    public class SynergyCharacterPointer : MonoBehaviour {

        private CharacterCtrl characterCtrl;

        private Dictionary<SynergyData, Transform> points = new Dictionary<SynergyData, Transform>();

        private Dictionary<SynergyData, float> yGlobalPositions = new Dictionary<SynergyData, float>();

        private void Reset() {

            if (characterCtrl == null && transform.parent != null)
                characterCtrl = transform.parent.gameObject.GetComponent<CharacterCtrl>();

        }

        public Transform AddOrUpdatePoint(SynergyData synData, float yGlobalPosition) {

            if (points.TryGetValue(synData, out var tr)) { 
                
                tr.position = new Vector3(transform.position.x, yGlobalPosition, transform.position.z);
                yGlobalPositions[synData] = yGlobalPosition;
                return tr;

            }

            var obj = new GameObject("SynergyPoint");
            obj.transform.SetParent(transform);
            obj.transform.position = new Vector3(transform.position.x, yGlobalPosition, transform.position.z);

            points.Add(synData, obj.transform);

            yGlobalPositions.Add(synData, yGlobalPosition);

            return obj.transform;

        }

        private void Update() {

            foreach (var kv in points) {
                kv.Value.position = new Vector3(kv.Value.position.x, yGlobalPositions[kv.Key], kv.Value.position.z);
            }

        }

    }

}
