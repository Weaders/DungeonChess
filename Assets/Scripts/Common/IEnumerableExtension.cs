using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Common {

    public static class IEnumerableExtension {

        public static T MinElement<T, TForCompare>(this IEnumerable<T> source, Func<T, TForCompare> forCompare) where TForCompare : IComparable<TForCompare> {

            var enumerator = source.GetEnumerator();

            TForCompare min = default;
            T element = default;

            if (enumerator.MoveNext()) {

                min = forCompare(enumerator.Current);
                element = enumerator.Current;

            }

            while (enumerator.MoveNext()) {

                var nextVal = forCompare(enumerator.Current);

                if (nextVal.CompareTo(min) < 0) {

                    min = nextVal;
                    element = enumerator.Current;

                }

            }

            return element;

        }

        public static T MaxElement<T, TForCompare>(this IEnumerable<T> source, Func<T, TForCompare> forCompare) where TForCompare : IComparable<TForCompare> {

            var enumerator = source.GetEnumerator();

            TForCompare min = default;
            T element = default;

            if (enumerator.MoveNext()) {

                min = forCompare(enumerator.Current);
                element = enumerator.Current;

            }

            while (enumerator.MoveNext()) {

                var nextVal = forCompare(enumerator.Current);

                if (nextVal.CompareTo(min) > 0) {

                    min = nextVal;
                    element = enumerator.Current;

                }

            }

            return element;

        }

        public static T RandomElement<T>(this IEnumerable<T> elements) {

            var count = elements.Count();

            var randomIndex = UnityEngine.Random.Range(0, count);

            var enumerator = elements.GetEnumerator();

            enumerator.MoveNext();

            while (randomIndex-- > 0) {
                enumerator.MoveNext();
            }

            return enumerator.Current;

        }

    }
}
