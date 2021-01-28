using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common {

    public class ComparerVector2IntByY : IComparer<Vector2Int> {

        public int Compare(Vector2Int a, Vector2Int b) {

            var firstCompare = a.y.CompareTo(b.y);

            if (firstCompare == 0)
                return a.x.CompareTo(b.x);

            return firstCompare;

        }
    }

    public class ComparerVector3ByX : IComparer<Vector3> {

        public int Compare(Vector3 a, Vector3 b) {

            var aY = Mathf.RoundToInt(a.y);
            var bY = Mathf.RoundToInt(b.y);

            var compare = aY.CompareTo(bY);

            if (compare == 0)
                return a.x.CompareTo(b.x);

            return compare;

        }

    }

}
