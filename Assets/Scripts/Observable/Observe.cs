using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

    }

    #endregion

    #region Observable val
    public interface IObservableVal { }

    public abstract class ObservableVal : IObservableVal {

        public abstract OrderedEventsBase onPreChangeBase { get; }

        public abstract OrderedEventsBase onPostChangeBase { get; }

    }

    public interface IModifiedObservable<T> {
        void Modify(ObservableVal<T> modifVal, bool alterVal = false);
    }

    [Serializable]
    public class ObservableVal<T> : ObservableVal {

        public ObservableVal(T currentVal) {
            val = currentVal;
        }

        [SerializeField]
        private T _val;

        public T val {
            get => _val;
            set {

                var oldVal = _val;

                onPreChange.Invoke(new ChangeData(oldVal, value));
                _val = value;
                onPostChange.Invoke(new ChangeData(oldVal, value));

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

    [Serializable]
    public class FloatObsrevable : ObservableVal<float>, IModifiedObservable<float> {

        public FloatObsrevable(float val) : base(val) { }



        public void Modify(ObservableVal<float> modifVal, bool alterVal = false) {
            val += alterVal ? -modifVal : modifVal;
        }
    }

    [Serializable]
    public class IntObservable : ObservableVal<int>, IModifiedObservable<int> {

        public IntObservable(int val) : base(val) { }

        public void Modify(ObservableVal<int> modifVal, bool alterVal = false) {
            val += alterVal ? -modifVal : modifVal;
        }

    }

    [Serializable]
    public class BoolObservable : ObservableVal<bool>, IModifiedObservable<bool> {

        public BoolObservable(bool val) : base(val) { }

        public void Modify(ObservableVal<bool> modifVal, bool alterVal = false) {
            val = alterVal ? !modifVal : modifVal;
        }
    }

    [Serializable]
    public class ArrayObservable<T> : ObservableVal<T[]>, IModifiedObservable<T[]> {

        public ArrayObservable(T[] val) : base(val) { }

        public void Modify(ObservableVal<T[]> modifVal, bool alterVal = false) {
            val = (alterVal ? val.Except(modifVal.val) : val.Union(modifVal.val)).ToArray();
        }
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

    //public interface IStatChangeData<T> {
    //    T val { get; }
    //}

    //public readonly struct StatChangeTextResult {

    //    public readonly string text;
    //    public readonly Color color;

    //    public StatChangeTextResult(string t, Color c) {
    //        text = t;
    //        color = c;
    //    }
    //}

    //public class DisplayOpts {

    //    public string source { get; private set; }

    //    public bool onlyInFight { get; set; }

    //    public Sprite sprite => Resources.Load<Sprite>($"UI/{source}");

    //    public DisplayOpts(string _source, bool isDisplayOnlyInFight = false) {

    //        source = _source;
    //        onlyInFight = isDisplayOnlyInFight;

    //    }

    //}

    //public interface IStat {

    //    DisplayOpts displayOpts { get; }

    //    UnityEvent noticeForChange { get; }


    //}

    //public abstract class Stat<T, ChangeStatData> : IStat where ChangeStatData : IStatChangeData<T> {

    //    [SerializeField]
    //    protected T _val;

    //    public T val {
    //        get => _val;
    //        set {

    //            var oldVal = _val;

    //            _val = value;

    //            onPostChangeForBuff.Invoke(oldVal, _val);
    //            onPostChangeForSpells.Invoke(oldVal, _val);
    //            onPostChangeForGame.Invoke(oldVal, _val);

    //            noticeForChange.Invoke();

    //        }
    //    }

    //    /// <summary>
    //    /// Called after others events
    //    /// </summary>
    //    public UnityEvent noticeForChange { get; } = new UnityEvent();

    //    /// <summary>
    //    /// This events is called first
    //    /// </summary>
    //    public ValChange onPostChangeForBuff { get; set; } = new ValChange();

    //    /// <summary>
    //    /// This events is called second
    //    /// </summary>
    //    public ValChange onPostChangeForSpells { get; set; } = new ValChange();

    //    /// <summary>
    //    /// This events is called third
    //    /// </summary>
    //    public ValChange onPostChangeForGame { get; set; } = new ValChange();

    //    public DisplayOpts displayOpts { get; protected set; }

    //    public Stat<T, ChangeStatData> maxStat { get; set; }

    //    public static implicit operator T(Stat<T, ChangeStatData> stat) => stat.val;

    //    public static implicit operator string(Stat<T, ChangeStatData> stat) => stat.ToString();

    //    public abstract StatChangeTextResult? GetChangeText(ChangeStatData changeVal);

    //    public override string ToString() {

    //        if (maxStat != null)
    //            return $"{val}/{maxStat}";

    //        return val.ToString();

    //    }

    //    public class ValChange : UnityEvent<T, T> { }

    //}

    //#region IntStat

    //[Serializable]
    //public class IntStat : Stat<int, IntStatChangeData> {

    //    public string statTranslate { get; protected set; }

    //    public IntStat(string translateName, int initVal = default, DisplayOpts opts = null) {
    //        val = initVal;
    //        displayOpts = opts;
    //        statTranslate = translateName ?? throw new ArgumentNullException(translateName);
    //    }

    //    public static int operator -(IntStat stat, IntStat stat2) => stat.val - stat2.val;

    //    public static int operator +(IntStat stat, IntStat stat2) => stat.val + stat2.val;

    //    public override StatChangeTextResult? GetChangeText(IntStatChangeData data) {

    //        var color = data.val > 0 ? TextColorStore.GoodEffect : TextColorStore.BadEffect;

    //        var text = TranslateReader.GetTranslate("stat_int_change", new System.Collections.Generic.Dictionary<string, string> {
    //            ["stat"] = TranslateReader.GetTranslate(statTranslate),
    //            ["change_val"] = data.DisplayChangeVal()
    //        });

    //        return new StatChangeTextResult(text, color);

    //    }

    //}

    //public readonly struct IntStatChangeData : IStatChangeData<int> {

    //    public int val { get; }

    //    public bool isPercent { get; }

    //    public IntStatChangeData(int val, bool isPercent = false) {

    //        this.val = val;
    //        this.isPercent = isPercent;

    //    }

    //    public string DisplayChangeVal() {

    //        if (isPercent) {
    //            return val + "%";
    //        } else {
    //            return val.ToString();
    //        }

    //    }


    //}

    //#endregion

    //#region BoolStat

    //public readonly struct BoolStatChangeData : IStatChangeData<bool> {
    //    public bool val { get; }
    //}

    //[Serializable]
    //public class BoolStat : Stat<bool, BoolStatChangeData> {

    //    public BoolStat(bool boolVal = default) {
    //        val = boolVal;
    //    }

    //    public override StatChangeTextResult? GetChangeText(BoolStatChangeData changeVal) => null;
    //}

    //#endregion

}
