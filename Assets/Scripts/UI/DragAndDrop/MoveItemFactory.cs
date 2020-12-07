using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.DragAndDrop {

    public interface IForMoveItem {
        void ClickHandle(MoveItem moveItem);
        Sprite img { get; }
    }

    public class MoveItemFactory : MonoBehaviour {

        public enum ReasonCreate {
            Inventory
        }

        [SerializeField]
        private MoveItem moveItemPrefab;

        private Dictionary<ReasonCreateObj, MoveItem> moveItems = new Dictionary<ReasonCreateObj, MoveItem>();

        public MoveItem CreateOrGet(ReasonCreate reason, IForMoveItem itemData) {

            var obj = new ReasonCreateObj(reason, itemData);

            if (moveItems.TryGetValue(obj, out var val)) {
                return val;
            }

            var moveItemObj = Instantiate(moveItemPrefab.gameObject, transform);

            var moveItem = moveItemObj.GetComponent<MoveItem>();

            moveItemObj.GetComponent<Image>().sprite = itemData.img;

            moveItems.Add(obj, moveItem);

            moveItem.onDestoy.AddListener(() => {
                moveItems.Remove(obj);
            });

            return moveItem;

        }

        /// <summary>
        /// Get <see cref="IForMoveItem"/>, by move item
        /// </summary>
        /// <param name="mv"></param>
        /// <returns></returns>
        public IForMoveItem Get(MoveItem mv) => GetKey(mv)?.forMoveItem;

        private ReasonCreateObj GetKey(MoveItem mv) {

            foreach (var kv in moveItems) {

                if (kv.Value == mv) {
                    return kv.Key;
                }

            }

            return default;

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

        private class ReasonCreateObj {

            public ReasonCreate reason;
            public IForMoveItem forMoveItem;

            public ReasonCreateObj(ReasonCreate r, IForMoveItem f) {
                forMoveItem = f;
                reason = r;
            }

            public override bool Equals(object obj) {

                if (obj is ReasonCreateObj rco) {
                    return rco.reason == reason && rco.forMoveItem == forMoveItem;
                }

                return base.Equals(obj);
            }

            public override int GetHashCode() {

                unchecked {

                    var prime = 37;

                    if (forMoveItem != null) {
                        prime ^= forMoveItem.GetHashCode();
                        prime *= 397;
                    }

                    prime ^= reason.GetHashCode();
                    prime *= 397;

                    return prime;

                }
                
            }

        }

    }

}
