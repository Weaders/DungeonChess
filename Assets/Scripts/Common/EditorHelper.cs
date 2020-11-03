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

    }
}
