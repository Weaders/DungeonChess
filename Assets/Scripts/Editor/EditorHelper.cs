using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Common {
    public class EditorHelper : MonoBehaviour {

        [MenuItem("Custom/Objects/Rotate90Y")]
        public static void Rotate90Y() {

            foreach (var gameObj in Selection.gameObjects) {
                gameObj.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            }

        }

        [MenuItem("Debug/Print Global Position")]
        public static void PrintGlobalPosition() {
            if (Selection.activeGameObject != null) {
                Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.transform.position);
            }

        }

        [MenuItem("Debug/Remove Player Prefs")]
        public static void RemovePlayerPrefs() {
            PlayerPrefs.DeleteAll();
        }
    }
}
