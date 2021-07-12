using System;
using System.Collections;

namespace Assets.Scripts.Common.Exceptions {
    public class GameException : Exception {

        public GameException(string msg, params object[] data) : base(msg) {

        }

        public override string ToString() => Message;

    }
}
