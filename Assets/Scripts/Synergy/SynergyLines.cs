using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.Synergy {

    [ExecuteInEditMode]
    public class SynergyLines : MonoBehaviour {

        private readonly float yOffset = 0.3f;

        [SerializeField]
        public LineRenderer linePrefab;

        private Dictionary<SynergyData, LineRendererFollowUp> lineRenders = new Dictionary<SynergyData, LineRendererFollowUp>();

        private Dictionary<SynergyData, float> offsets = new Dictionary<SynergyData, float>();

        public GameObject GetPointFor(SynergyData synergyData, params CharacterCtrl[] ctrls) {

            var offset = CreateOrGetOffset(synergyData);

            IEnumerable<Transform> trs = ctrls
                .Select(ctrl => ctrl.synergyCharacterPoint.AddOrUpdatePoint(synergyData, transform.position.y + offset));

            var minTr = trs.MinElement(tr => tr.position.y + tr.position.x + tr.position.z);

            trs = trs.OrderBy(tr => Vector3.Distance(minTr.position, tr.position));

            if (lineRenders.TryGetValue(synergyData, out var followUp)) {

                followUp.SetTransforms(trs.ToArray());
                return followUp.gameObject;

            } else {

                var obj = Instantiate(linePrefab.gameObject, transform);

                var lineRenderer = obj.GetComponent<LineRenderer>();

                followUp = obj.GetComponent<LineRendererFollowUp>();

                lineRenderer.endColor = lineRenderer.startColor = synergyData.GetLineColor();

                followUp.SetTransforms(trs.ToArray());

                lineRenders.Add(synergyData, followUp);

                return followUp.gameObject;

            }

        }

        private float CreateOrGetOffset(SynergyData synergyData) {

            if (offsets.TryGetValue(synergyData, out var toReturn)) {
                return toReturn;
            }

            var newOffset = yOffset;

            if (offsets.Any()) {
                newOffset = offsets.Values.Last() + yOffset;
            }

            TagLogger<SynergyLines>.Info($"Add new offset {newOffset}");

            offsets.Add(synergyData, newOffset);

            return newOffset;

        }

    }
}
