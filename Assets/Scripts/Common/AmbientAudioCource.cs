using UnityEngine;

namespace Assets.Scripts.Common {
    public class AmbientAudioCource : MonoBehaviour {

        private Camera mainCamera;

        private void Awake() {

            if (FindObjectsOfType<AmbientAudioCource>().Length > 1) {

                Destroy(gameObject);
                return;

            }


            DontDestroyOnLoad(gameObject);
            mainCamera = Camera.main;

        }

        private void Update() {

            if (mainCamera == null)
                mainCamera = Camera.main;

            gameObject.transform.position = mainCamera.transform.position;
        }

    }
}
