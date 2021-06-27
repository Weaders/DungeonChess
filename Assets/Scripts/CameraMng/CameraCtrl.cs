using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.CameraMng {

    public class CameraCtrl : MonoBehaviour {

        [SerializeField]
        private float speed;

        [SerializeField]
        private Vector3 offsetToRoom;

        private List<GameObject> touchObjects = new List<GameObject>();

        private IEnumerator GoToPosition(Vector3 targetPos) {

            var t = 0f;

            while (t < 1f) {

                transform.position = Vector3.Lerp(transform.position, targetPos, t);

                t += speed / 10f * Time.deltaTime;

                if (t > 1f)
                    t = 1f;

                yield return new WaitForFixedUpdate();

            }

        }

    }
}
