using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Common {
    public static class IteratorEnumerable {

        public static IEnumerable<T> Iterate<T>(params IEnumerable<T>[] enumerate) {

            foreach (var array in enumerate)
                foreach(var element in array)
                    yield return element;

        }

    }
}
