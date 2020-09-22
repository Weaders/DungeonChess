using System;

namespace Assets.Scripts.Common.Exceptions {
    public class GameException : Exception {

        private string _msg;

        public GameException(string msg) {
            _msg = msg;
        }

        public override string ToString() => _msg;

    }
}
