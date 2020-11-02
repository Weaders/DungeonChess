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

        public static (T, float) ClosestElement<T, G>(this IEnumerable<T> elements, G forElement, Func<T, G, float> getDistance, IEnumerable<T> ignoreElements = null)  {

            if (!elements.Any())
                return (default, default);

            var enumerator = elements.GetEnumerator();
            T forReturn = default;

            var minDistance = float.MaxValue;

            while (enumerator.MoveNext()) {

                if (!enumerator.Current.Equals(forElement) && (ignoreElements == null || !ignoreElements.Contains(enumerator.Current))) {

                    var distance = getDistance(enumerator.Current, forElement);

                    if (minDistance > distance) {

                        minDistance = distance;
                        forReturn = enumerator.Current;

                    }

                }

            }

            return (forReturn, minDistance);

        }

    }
}
