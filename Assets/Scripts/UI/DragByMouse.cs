using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI {
    public class DragByMouse : MonoBehaviour, IDragHandler {

        [SerializeField]
        private float moveSpeed;

        private Camera cameraForMove;

        private void Awake() {
            cameraForMove = Camera.main;
        }

        public void OnDrag(PointerEventData eventData) {

            if (GameMng.current.gameInputCtrl.draggedCtrl == null)
                cameraForMove.transform
                    .Translate(new Vector3(-eventData.delta.x, 0, -eventData.delta.y) * moveSpeed * Time.deltaTime, 
                    Space.World
                );

        }

    }

}
