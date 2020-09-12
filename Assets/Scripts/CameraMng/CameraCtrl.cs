﻿using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.CameraMng {

    public class CameraCtrl : MonoBehaviour {

        [SerializeField]
        private float speed;

        private List<GameObject> touchObjects = new List<GameObject>();

        private void Update() {

            var horizontalVal = Input.GetAxisRaw("Horizontal");
            var verticallVal = Input.GetAxisRaw("Vertical");

            if (horizontalVal != 0 || verticallVal != 0) {

                transform.Translate(
                    new Vector3(
                        horizontalVal * speed * Time.deltaTime, 
                        0, 
                        verticallVal * speed * Time.deltaTime
                    )
                );
            }

            //var hits = Physics.SphereCastAll(transform.position, 10f, transform.forward, 10f, LayerMask.GetMask(LayersStore.WALL_LAYER));

            //var touches = new bool[touchObjects.Count];

            //foreach(var hit in hits) {

            //    //var index = touchObjects.IndexOf(hit.collider.gameObject);

            //    //if (index < 0) {

            //        //touchObjects.Add(hit.collider.gameObject);

            //        Debug.DrawLine(transform.position, hit.point, Color.red);

            //        //hit.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
            //        //TagLogger<CameraCtrl>.Info("Hit wall object with name " + hit.collider.gameObject.name);

            //    //} else {
            //    //    touches[index] = true;
            //    //}

            //}

            //var offset = 0;

            //for (var i = 0; i < touches.Length; i++) {

            //    if (!touches[i]) {

            //        touchObjects[i - offset].GetComponent<MeshRenderer>().enabled = true;
            //        touchObjects.RemoveAt(i- offset);
            //        offset++;

            //    }

            //}

        }

    }
}
