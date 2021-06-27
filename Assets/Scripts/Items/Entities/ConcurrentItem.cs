using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public abstract class ConcurrentItem : ItemData {

        [SerializeField]
        private int _countItems;

        private ObservableVal<int> countVal = new ObservableVal<int>();

        public override ObservableVal<int> count => countVal;

        private void Awake() {
            countVal.val = _countItems;
        }

        protected override void Equip() {

            Use(owner);
            countVal.val--;

            if (countVal == 0) {

                Destroy(gameObject);

                if (owner != null) {

                    for (var i = 0; i < owner.itemsContainer.Count; i++) {

                        if (owner.itemsContainer[i] == this) {

                            owner.itemsContainer[i] = null;
                            break;

                        }

                    }

                }

            }

        }

        protected override void OnDeEquip() {

        }

        public override bool IsNeedMoveBack() {
            return owner != null;
        }

        protected abstract void Use(CharacterData data);
    }
}
