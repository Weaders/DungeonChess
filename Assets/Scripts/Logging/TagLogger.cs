using UnityEngine;

namespace Assets.Scripts.Logging {

    public class TagLogger<T> {

        public static void Info(string msg) {
            Debug.Log($"[{typeof(T).Name}] {msg}");
        }

        public static void Error(params object[] msgs) {

            foreach (var msg in msgs)
                Debug.LogError(msg);

        }

        public static void InfoMany(params object[] msgs) {
            foreach (var msg in msgs)
                Info(msg.ToString());
        }

    }

}
