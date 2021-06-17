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

            if (_countItems == 0)
                Destroy(gameObject);
            else
                DeEquip();

        }

        protected override void OnDeEquip() {
            
        }

        protected abstract void Use(CharacterData data);
    }
}
