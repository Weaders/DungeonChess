using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StarsData;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Observable {

    public enum OrderVal {
        Internal,
        UIUpdate,
        ContainersUpdate,
        Fight,
        CharacterCtrl,
        Buff
    }

    #region Event data
    public abstract class BaseOrderedEventData {

        public BaseOrderedEventData(OrderVal orderVal) {
            order = orderVal;
        }

        public readonly OrderVal order;
    }

    public class OrderedEventData : BaseOrderedEventData {

        public OrderedEventData(OrderVal orderVal) : base(orderVal) { }

        public UnityEvent unityEvent = new UnityEvent();

    }

    public class OrderedEventData<T> : BaseOrderedEventData where T : class {

        public OrderedEventData(OrderVal orderVal) : base(orderVal) { }

        public UnityEvent<T> unityEvent = new UnityEvent<T>();

    }

    #endregion

    #region Ordered events
    public abstract class OrderedEventsBase {

        protected LinkedListNode<T> SearchNode<T>(LinkedListNode<T> firstItem, OrderVal order) where T : BaseOrderedEventData {

            var current = firstItem;

            while (current != null) {

                if (current.Value.order == order) {
                    return current;
                } else if (current.Value.order > order) {
                    break;
                }


                current = current.Next;

            }

            return null;

        }

        public abstract void AddSubscription(OrderVal order, UnityAction unityAction);

        public abstract void RemoveSubscription(UnityAction act);

    }

    public class OrderedEvents : OrderedEventsBase {

        private LinkedList<OrderedEventData> events = new LinkedList<OrderedEventData>();

        public override void AddSubscription(OrderVal order, UnityAction unityAction) {

            var current = SearchNode(events.First, order);

            if (current == null) {

                var obj = events.AddFirst(new OrderedEventData(order));
                obj.Value.unityEvent.AddListener(unityAction);

            } else if (current.Value.order == order) {

                current.Value.unityEvent.AddListener(unityAction);

            } else {

                var added = events.AddBefore(current, new OrderedEventData(order));
                added.Value.unityEvent.AddListener(unityAction);

            }

        }

        public override void RemoveSubscription(UnityAction act) {

            foreach (var e in events) {
                e.unityEvent.RemoveListener(act);
            }

        }

        public void Invoke() {

            foreach (var eventObj in events) {
                eventObj.unityEvent.Invoke();
            }

        }

    }

    public class OrderedEvents<T> : OrderedEventsBase where T : class {

        private LinkedList<OrderedEventData<T>> events = new LinkedList<OrderedEventData<T>>();

        private Dictionary<UnityAction, UnityAction<T>> actPointers = new Dictionary<UnityAction, UnityAction<T>>();

        public void AddSubscription(OrderVal order, UnityAction<T> action) {

            var current = SearchNode(events.First, order);

            if (current == null) {

                var obj = events.AddFirst(new OrderedEventData<T>(order));
                obj.Value.unityEvent.AddListener(action);

            } else if (current.Value.order == order) {

                current.Value.unityEvent.AddListener(action);

            } else {

                var added = events.AddBefore(current, new OrderedEventData<T>(order));
                added.Value.unityEvent.AddListener(action);

            }

        }

        public void RemoveSubscription(UnityAction<T> action) {

            foreach (var e in events) {
                e.unityEvent.RemoveListener(action);
            }

        }

        public override void AddSubscription(OrderVal order, UnityAction unityAction) {

            void newAction(T _) { unityAction.Invoke(); }

            actPointers.Add(unityAction, newAction);

            AddSubscription(order, newAction);
        }


        public override void RemoveSubscription(UnityAction act) {

            if (actPointers.TryGetValue(act, out var newAct)) {

                foreach (var e in events) {
                    e.unityEvent.RemoveListener(newAct);
                }

            }

        }

        public void Invoke(T val) {

            foreach (var eventObj in events) {
                eventObj.unityEvent.Invoke(val);
            }

        }

    }
    #endregion

    #region Observable array

    public interface IObservableArray<T> : IEnumerable<T> {

        OrderedEvents<ChangeEnumerableItemEvent<T>> onSet { get; }

        T this[int index] { get; set; }

        int Count { get; }

        void Clear();

    }

    [Serializable]
    public class ObservableArray<T> : IObservableArray<T> {

        private T[] items;

        public ObservableArray(int size) {
            items = new T[size];
        }

        public T this[int index] {
            get => items[index];
            set {

                if ((items[index] == null && value != null) || (items[index] != null && !items[index].Equals(value))) {

                    items[index] = value;
                    onSet.Invoke(new ChangeEnumerableItemEvent<T>(value, index));

                }

            }
        }

        public OrderedEvents<ChangeEnumerableItemEvent<T>> onSet => _onSet;

        private OrderedEvents<ChangeEnumerableItemEvent<T>> _onSet = new OrderedEvents<ChangeEnumerableItemEvent<T>>();

        public int Count => items.Length;

        public void Clear() {
            for (var i = 0; i < Count; i++) {
                this[i] = default;
            }
        }

        public int GetIndex(T data) => Array.IndexOf(items, data);

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator() as IEnumerator<T>;

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }

    #endregion

    #region Observable List
    public interface IObservableList<T> : IReadOnlyList<T> {

        OrderedEvents<ChangeEnumerableItemEvent<T>> onAdd { get; }
        OrderedEvents<ChangeEnumerableItemEvent<T>> onRemove { get; }

        void Add(T data);
        void Remove(T data);
    }

    [Serializable]
    public class ObservableList<T> : IObservableList<T> {

        private List<T> _list = new List<T>();

        public ObservableList() { }

        public ObservableList(int size) {
            _list = new List<T>(new T[size]);
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public OrderedEvents<ChangeEnumerableItemEvent<T>> onAdd { get; set; } = new OrderedEvents<ChangeEnumerableItemEvent<T>>();

        public OrderedEvents<ChangeEnumerableItemEvent<T>> onRemove { get; set; } = new OrderedEvents<ChangeEnumerableItemEvent<T>>();

        public int Count => _list.Count;

        public T this[int index] {
            get => _list[index];
            set {
                _list[index] = value;
                onAdd.Invoke(new ChangeEnumerableItemEvent<T>(value, index));
            }
        }

        public virtual void Add(T data) {

            _list.Add(data);
            onAdd.Invoke(new ChangeEnumerableItemEvent<T>(data, _list.Count - 1));

        }

        public virtual void Remove(T data) {

            _list.Remove(data);
            onRemove.Invoke(new ChangeEnumerableItemEvent<T>(data, _list.Count));

        }

        public virtual void Remove(int index) {
            _list.RemoveAt(index);
        }

        public virtual void Clear() {

            var count = _list.Count;

            _list.Clear();

            for (var i = 0; i < count; i++) {
                onRemove.Invoke(new ChangeEnumerableItemEvent<T>(default, i));
            }


        }

        public virtual T GetOrDefault(int index) {

            if (_list.Count > index) {
                return _list[index];
            }

            return default;

        }

        public virtual int GetIndex(T data) => _list.IndexOf(data);

    }

    #endregion

    #region Observable val
    public interface IObservableVal {

        OrderedEventsBase onPreChangeBase { get; }
        OrderedEventsBase onPostChangeBase { get; }

    }

    [Serializable]
    public class ObservableVal : IObservableVal {

        public virtual OrderedEventsBase onPreChangeBase { get; }

        public virtual OrderedEventsBase onPostChangeBase { get; }

    }

    [Serializable]
    public class ObservableVal<T> : ObservableVal {

        public ObservableVal() {
            val = default(T);
        }

        public ObservableVal(T currentVal) {
            val = currentVal;
        }

        [SerializeField]
        private T _val;

        public T val {
            get => _val;
            set {

                var oldVal = _val;

                if ((oldVal == null && value != null) || (oldVal != null && !oldVal.Equals(value))) {

                    onPreChange.Invoke(new ChangeData(oldVal, value));
                    _val = value;
                    onPostChange.Invoke(new ChangeData(oldVal, value));

                }
            }
        }

        public override OrderedEventsBase onPreChangeBase => onPreChange;
        public override OrderedEventsBase onPostChangeBase => onPostChange;

        public OrderedEvents<ChangeData> onPreChange = new OrderedEvents<ChangeData>();

        public OrderedEvents<ChangeData> onPostChange = new OrderedEvents<ChangeData>();

        public override string ToString() => val.ToString();

        public static implicit operator T(ObservableVal<T> obj) => obj.val;

        public class ChangeData {

            public ChangeData(T _oldVal, T _newVal) {

                oldVal = _oldVal;
                newVal = _newVal;

            }

            public T oldVal { get; }
            public T newVal { get; }

        }

    }

    public interface IModifiedObservable { }

    public interface IModifiedObservable<T> : IModifiedObservable {
        void Modify(ObservableVal<T> modifVal, bool alterVal = false);
        void ModifyTarget(ObservableVal<T> target, bool alterVal = false);
    }

    [Serializable]
    public class FloatObsrevable : ObservableVal<float>, IModifiedObservable<float> {

        public FloatObsrevable(float val) : base(val) { }

        public void Modify(ObservableVal<float> modifVal, bool alterVal = false) {
            val += alterVal ? -modifVal : modifVal;
        }

        public void ModifyTarget(ObservableVal<float> target, bool alterVal = false) {
            target.val += alterVal ? -val : val;
        }
    }

    [Serializable]
    public class IntObservable : ObservableVal<int>, IModifiedObservable<int> {

        public IntObservable(int val) : base(val) { }

        public void Modify(ObservableVal<int> modifVal, bool alterVal = false) {
            val += alterVal ? -modifVal : modifVal;
        }

        public void ModifyTarget(ObservableVal<int> target, bool alterVal = false) {
            target.val += alterVal ? -val : val;
        }

    }

    [Serializable]
    public class BoolObservable : ObservableVal<bool>, IModifiedObservable<bool> {

        public BoolObservable(bool val) : base(val) { }

        public void Modify(ObservableVal<bool> modifVal, bool alterVal = false) {
            val = alterVal ? !modifVal : modifVal;
        }

        public void ModifyTarget(ObservableVal<bool> target, bool alterVal = false) {
            target.val = alterVal ? !val : val; ;
        }
    }

    [Serializable]
    public class ArrayObservable<T> : ObservableVal<T[]>, IModifiedObservable<T[]> {

        public ArrayObservable(T[] val) : base(val) { }

        public void Modify(ObservableVal<T[]> modifVal, bool alterVal = false) {
            val = (alterVal ? val.Except(modifVal.val) : val.Union(modifVal.val)).ToArray();
        }

        public void ModifyTarget(ObservableVal<T[]> target, bool alterVal = false) {
            target.val = (alterVal ? target.val.Except(val) : target.val.Union(val)).ToArray();
        }
    }

    [Serializable]
    public class CharacterClassTypeObservable : ArrayObservable<CharacterClassType> {
        public CharacterClassTypeObservable(CharacterClassType[] val) : base(val) { }
    }

    #endregion



    public class ChangeEnumerableItemEvent<T> {

        public ChangeEnumerableItemEvent(T newData, int index) {
            data = newData;
            this.index = index;
        }

        public T data { get; set; }
        public int index { get; set; }

    }

}
