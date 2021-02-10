using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI {
    public class DragByMouse : MonoBehaviour, IDragHandler, IEndDragHandler {

        [SerializeField]
        private float moveSpeed;

        private Camera cameraForMove;

        [SerializeField]
        private float zOffset = 5f;

        public bool isDragged { get; private set; }

        private void Awake() {
            cameraForMove = Camera.main;
        }

        public void OnDrag(PointerEventData eventData) {

            if (GameMng.current.gameInputCtrl.draggedCtrl == null && !GameMng.current.messagePanel.IsShowed && !GameMng.current.selectPanel.IsShowed) {

                // Use it there better, than use OnBeginDrag
                isDragged = true;

                var delta = new Vector3(-eventData.delta.x, 0, -eventData.delta.y) * moveSpeed * Time.deltaTime;

                var newX = Mathf.Clamp(
                    cameraForMove.transform.position.x + delta.x, 
                    GameMng.current.roomCtrl.XZRect.xMin,
                    GameMng.current.roomCtrl.XZRect.xMax
                );

                var newZ = Mathf.Clamp(
                    cameraForMove.transform.position.z + delta.z,
                    GameMng.current.roomCtrl.XZRect.yMin - zOffset,
                    GameMng.current.roomCtrl.XZRect.yMax - zOffset
                );

                cameraForMove.transform.position = new Vector3(
                    newX, 
                    cameraForMove.transform.position.y, 
                    newZ
                );

            }


        }

        public void OnEndDrag(PointerEventData eventData) {
            Invoke(nameof(SetStopDragged), .1f);
        }

        private void SetStopDragged() {
            isDragged = false;
        }

    }

}
