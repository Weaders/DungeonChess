using UnityEngine;

namespace Assets.Scripts.Common {

    public static class CellRangeHelper {

        public static bool IsInRange(Vector2 from, Vector2 to, int range) {

            var diff = (from - to);
            return Mathf.Abs(diff.x) <= range && Mathf.Abs(diff.y) <= range;

        }

    }

}
