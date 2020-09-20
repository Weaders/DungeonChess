using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI {

    public interface IForMoveItem {
        void ClickHandle(MoveItem moveItem);
    }

    public class MoveItemFactory : MonoBehaviour {

        [SerializeField]
        private MoveItem moveItemPrefab;

        private Dictionary<IForMoveItem, MoveItem> moveItems = new Dictionary<IForMoveItem, MoveItem>();

        public MoveItem CreateOrGet(IForMoveItem itemData) {

            if (moveItems.TryGetValue(itemData, out var val)) {
                return val;
            }

            var moveItemObj = Instantiate(moveItemPrefab.gameObject, transform);

            var moveItem = moveItemObj.GetComponent<MoveItem>();

            moveItems.Add(itemData, moveItem);

            return moveItem;

        }

        /// <summary>
        /// Get <see cref="IForMoveItem"/>, by move item
        /// </summary>
        /// <param name="mv"></param>
        /// <returns></returns>
        public IForMoveItem Get(MoveItem mv) {

            foreach (var kv in moveItems) {

                if (kv.Value == mv) {
                    return kv.Key;
                }

            }

            return null;

        }

        /// <summary>
        /// Get obj, by <see cref="MoveItem"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mv"></param>
        /// <returns></returns>
        public T Get<T>(MoveItem mv) where T : MonoBehaviour, IForMoveItem {

            var moveItem = Get(mv);

            if (moveItem != null) {
                return moveItem as T;
            }

            throw new ArgumentException("Try get item for move item, that not exists");

        }

        public void Remove(IForMoveItem mv) {

            if (moveItems.TryGetValue(mv, out var val)) {
                
                Destroy(val.gameObject);
                moveItems.Remove(mv);

            }
                
        }

    }
}
