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

    public class ItemsContainer : MonoBehaviour, IObservableList<ItemData> {

        private ObservableList<ItemData> items = new ObservableList<ItemData>();

        public ItemData this[int index] {
            get => items[index];
            set => items[index] = value;
        }

        public OrderedEvents<ChangeEnumerableItemEvent<ItemData>> onAdd => items.onAdd;

        public OrderedEvents<ChangeEnumerableItemEvent<ItemData>> onRemove => items.onRemove;




        public IHaveItemsContainer haveItemsContainer;

        public int Count => items.Count;

        public int maxItemsCount => _maxItemsCount;

        [SerializeField]
        private int _maxItemsCount = 4;

        public void SetOwner(IHaveItemsContainer haveItems) {

            haveItemsContainer = haveItems;

            items.Clear();
            items = new ObservableList<ItemData>(maxItemsCount);

            var i = 0;

            var enumerator = transform.GetEnumerator();

            while (enumerator.MoveNext()) {

                var item = (enumerator.Current as Transform).GetComponent<ItemData>();
                Set(item, i);
                i++;

            }

        }

        /// <summary>
        /// Add in first free cell
        /// </summary>
        /// <param name="data"></param>
        public void Add(ItemData data) {

            for (var i = 0; i < items.Count; i++) {

                if (items[i] == null) {
                    Set(data, i);
                    return;
                }

            }

            throw new GameException("There no free cells in items");

        }

        public void AddPrefab(ItemData prefab) {
            Add(Instantiate(prefab.gameObject).GetComponent<ItemData>());
        }

        public void Set(ItemData data, int index) {

            if (items[index] != null) {
                Remove(index);
            }

            items[index] = data;
            AfterAdd(data);

        }

        private void AfterAdd(ItemData data) {

            if (data != null) {

                data.transform.SetParent(transform);

                haveItemsContainer.OnAddItem(data);

            }

        }

        public IEnumerator<ItemData> GetEnumerator() => items.GetEnumerator();

        public void Remove(ItemData data) {

            items.Remove(data);
            data.transform.SetParent(null);
            haveItemsContainer.OnRemoveItem(data);

        }

        public void Remove(int index) {

            var item = items.GetOrDefault(index);

            if (item != null)
                Remove(item);

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
