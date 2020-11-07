using UnityEngine;

namespace Assets.Scripts.CellsGrid {

    public class CellOutlineAnim : MonoBehaviour {

        [SerializeField]
        private Material material;

        public float timeRepeat = 0.05f;

        public float progressChange = 0.1f;

        private float time;

        private void Start() {
            InvokeRepeating("ChangeMaterial", 0f, timeRepeat);
        }

        private void ChangeMaterial() {

            time += progressChange;

            material.SetFloat("_Progress", time);

            if (time >= 1f)
                time = 0f;

        }

    }
}
