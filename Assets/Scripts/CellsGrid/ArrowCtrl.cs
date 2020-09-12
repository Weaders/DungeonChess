using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.CellsGrid {

    public class ArrowCtrl : MonoBehaviour {

        public UnityEvent onClick = new UnityEvent();

        private void Update() {

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonUp(0) && Physics.Raycast(ray, out RaycastHit hit)) {

                onClick.Invoke();
                Debug.Log("Arrow click");

            }

        }


    }
}
