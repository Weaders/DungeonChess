using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

    [RequireComponent(typeof(Collider2D))]
    public class MoveItemCell : MonoBehaviour {

        public enum State {
            Default, AvaliableForSelect, NotAvailableForSelect
        }

        private State _state;

        private MoveItem _moveItem;

        public ChangeItemEvent onChangeItem = new ChangeItemEvent();

        [SerializeField]
        private Image image;

        public MoveItem moveItem {
            get => _moveItem;
            set {

                var oldVal = _moveItem;

                _moveItem = value;

                if (_moveItem != null) {
                    PlaceItem();
                }

                if (oldVal != _moveItem)
                    onChangeItem.Invoke(oldVal, _moveItem);

            }
        }

        public void InitWithItem(MoveItem moveItem) {
            
            _moveItem = moveItem;
            PlaceItem();

        }

        private void PlaceItem() {
            moveItem.transform.SetParent(transform);
            moveItem.transform.localPosition = Vector3.zero;
        }

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
