using System.Collections.Generic;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Common {
    public static class CharacterCtrlHelper {

        public static CharacterCtrl GetClosestCtrl(CharacterCtrl from, IEnumerable<CharacterCtrl> others) {

            var minDistance = float.MaxValue;
            CharacterCtrl selectedCtrl = null;

            foreach (var other in others) {

                var distance = Vector3.Distance(other.transform.position, from.transform.position);

                if (distance < minDistance) {

                    minDistance = distance;
                    selectedCtrl = other;

                }

            }

            return selectedCtrl;

        }

    }
}
