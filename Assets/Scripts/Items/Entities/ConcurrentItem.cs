using System;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.Items.Entities {

    public abstract class ConcurrentItem : ItemData {

        [SerializeField]
        private int _countItems;

        public int count { 
            get => _countItems; 
        }

        protected override void Equip() {
            
            Use(owner);
            _countItems--;

            Destroy(gameObject);

            if (count == 0) {

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
