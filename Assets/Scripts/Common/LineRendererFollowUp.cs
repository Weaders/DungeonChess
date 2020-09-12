using System.Linq;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.Common {

    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererFollowUp : MonoBehaviour {

        [SerializeField]
        private Transform[] _trs;

        private Vector3 _offset;

        [SerializeField]
        private LineRenderer lineRenderer;

        public void SetTransforms(Vector3 offset, params Transform[] trs) {
            //lineRenderer.positionCount
            TagLogger<LineRendererFollowUp>.Info($"Set up transforms for follow {trs.Length}");
            lineRenderer.positionCount = trs.Length;
            _trs = trs;
            _offset = offset;
        }

        private void Reset() {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update() {
            lineRenderer.SetPositions(_trs.Select(tr => tr.position + _offset).ToArray());
        }

    }
}
