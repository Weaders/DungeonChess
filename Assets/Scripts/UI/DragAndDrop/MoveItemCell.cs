using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI.DragAndDrop {

    [RequireComponent(typeof(Collider2D))]
    public class MoveItemCell : MonoBehaviour {

        public enum State {
            Default, AvaliableForSelect, NotAvailableForSelect
        }

        private State _state;
        public ChangeItemEvent onChangeItem = new ChangeItemEvent();

        public MoveItem moveItem { get; private set; }

        public void SetMoveItem(MoveItem newMoveItem, bool fireEvents = true) {

            var oldVal = moveItem;

            moveItem = newMoveItem;

            if (fireEvents)
                onChangeItem.Invoke(oldVal, newMoveItem);

        }

        [SerializeField]
        private Image image;

        public State state {
            get => _state;
            set {
                _state = value;
                OnChangeState();
            }
        }

        private void OnChangeState() {

            if (state == State.AvaliableForSelect) {
                image.color = Color.green;
            } else if (state == State.Default) {
                image.color = Color.white;
            } else if (state == State.NotAvailableForSelect) {
                image.color = Color.red;
            }

        }

        private void Reset() {
            image = GetComponent<Image>();
        }

        public class ChangeItemEvent : UnityEvent<MoveItem, MoveItem> {

        }

    }
}
