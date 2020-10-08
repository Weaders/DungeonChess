using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common {

    [ExecuteInEditMode]
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererFollowUp : MonoBehaviour {

        [SerializeField]
        private Transform[] _trs;

        [SerializeField]
        private LineRenderer lineRenderer;

        public Vector3 offset { get; set; }

        public void SetTransforms(params Transform[] trs) {
            lineRenderer.positionCount = trs.Length;
            _trs = trs;
        }

        private void Reset() {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update() {
            lineRenderer.SetPositions(_trs.Select(tr => tr.position).ToArray());
        }

    }
}
