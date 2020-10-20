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

        private Dictionary<SynergyData, CharacterCtrl[]> ctrlsForFollowUp = new Dictionary<SynergyData, CharacterCtrl[]>();

        public GameObject GetPointFor(SynergyData synergyData, params CharacterCtrl[] ctrls) {

            var offset = CreateOrGetOffset(synergyData);

            var trs = ctrls
                .Select(ctrl => ctrl.synergyCharacterPoint.AddOrUpdatePoint(synergyData, transform.position.y + offset).pointTransform)
                .OrderBy(d => d.position, new CustomComparer<Vector3>((a, b) => {
                    var aX = Mathf.RoundToInt(a.x);
                    var bX = Mathf.RoundToInt(b.x);

                    var compare = aX.CompareTo(bX);

                    if (compare == 0)
                        return a.z.CompareTo(b.z);

                    return compare;

                }));

            ctrlsForFollowUp[synergyData] = ctrls;

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
