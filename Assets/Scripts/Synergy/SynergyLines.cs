using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Synergy {

    public class SynergyLines : MonoBehaviour {

        [SerializeField]
        public LineRenderer linePrefab;

        public GameObject CreateLineFor(Color color, params CharacterCtrl[] ctrls) {

            var obj = Instantiate(linePrefab.gameObject, transform);
            var lineRenderer = obj.GetComponent<LineRenderer>();
            var followUp = obj.GetComponent<LineRendererFollowUp>();

            lineRenderer.material.color = color;

            followUp.SetTransforms(new Vector3(0, 2f, 0), ctrls.Select(ctrl => ctrl.transform).ToArray());
            //lineRenderer.startColor = lineRenderer.endColor = color;

            return obj;

        }

    }
}
