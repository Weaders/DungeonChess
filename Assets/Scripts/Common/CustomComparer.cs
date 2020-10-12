using System;
using System.Collections.Generic;

namespace Assets.Scripts.Common {

    public class CustomComparer<T> : IComparer<T> {

        private Func<T, T, int> Method { get; }

        public CustomComparer(Func<T, T, int> method) {
            Method = method;
        }

        public int Compare(T x, T y) => Method.Invoke(x, y);
    }
}
