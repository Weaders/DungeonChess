using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.Exceptions;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Items {

    public interface IHaveItemsContainer {

        void OnAddItem(ItemData item);

        void OnRemoveItem(ItemData item);

    }

    public class ItemsContainer : MonoBehaviour, IObservableArray<ItemData> {

        private ObservableArray<ItemData> items = new ObservableArray<ItemData>(0);

        public ItemData this[int index] {
            get => items[index];
            set {

                if (items[index] != value) {

                    if (items[index] != null && value != items[index]) {

                        haveItemsContainer.OnRemoveItem(items[index]);
                        items[index].transform.SetParent(null);

                    }

                    items[index] = value;

                    if (items[index] != null) {

                        items[index].transform.SetParent(transform);
                        haveItemsContainer.OnAddItem(items[index]);

                    }

                }
            }
        }

        public IHaveItemsContainer haveItemsContainer;

        public int Count => items.Count;

        public int maxItemsCount => _maxItemsCount;

        public OrderedEvents<ChangeEnumerableItemEvent<ItemData>> onSet => items.onSet;

        [SerializeField]
        private int _maxItemsCount = 4;

        public void SetOwner(IHaveItemsContainer haveItems) {

            haveItemsContainer = haveItems;

            Clear();

            items = new ObservableArray<ItemData>(maxItemsCount);

            var i = 0;

            var enumerator = transform.GetEnumerator();

            while (enumerator.MoveNext()) {

                var item = (enumerator.Current as Transform).GetComponent<ItemData>();
                this[i] = item;
                i++;

            }

        }

        /// <summary>
        /// Add in first free cell
        /// </summary>
        /// <param name="data"></param>
        public void Add(ItemData data) {

            for (var i = 0; i < Count; i++) {

                if (this[i] == null) {

                    this[i] = data;
                    return;

                }

            }

            throw new GameException("There no free cells in items");

        }

        public void AddPrefab(ItemData prefab) {
            Add(Instantiate(prefab.gameObject).GetComponent<ItemData>());
        }

        public IEnumerator<ItemData> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Clear() {
            for (var i = 0; i < Count; i++) {
                this[i] = null;
            }
        }
    }

}
