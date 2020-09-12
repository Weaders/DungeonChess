using Assets.Scripts.Common;
using Assets.Scripts.Logging;
using UnityEngine;

namespace Assets.Scripts.CameraMng {

    public class SphereView : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {
            TagLogger<SphereView>.Info("Colider with " + other.gameObject.name);

            if (other.gameObject.layer == LayerMask.NameToLayer(LayersStore.WALL_LAYER)) {
                other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer(LayersStore.WALL_LAYER)) {
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }

    }
}
