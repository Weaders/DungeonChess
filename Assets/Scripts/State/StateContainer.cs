using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.State {

    public class StateContainer : MonoBehaviour, IObservableList<StateData> {

        private ObservableList<StateData> items = new ObservableList<StateData>(0);

        public CharacterData owner { get; private set; }

        public StateData this[int index] { get => items[index]; set => items[index] = value; }

        public int Count => items.Count;

        public OrderedEvents<ChangeEnumerableItemEvent<StateData>> onAdd => items.onAdd;

        public OrderedEvents<ChangeEnumerableItemEvent<StateData>> onRemove => items.onRemove;

        public void Add(StateData stateData) {
            
            items.Add(stateData);
            stateData.transform.SetParent(transform);
            stateData.Apply(owner.characterCtrl);

        }

        public void Init(CharacterData characterCtrl) {
            owner = characterCtrl;
        }

        public StunState AddStun(float time) {

            var gameObj = new GameObject();
            
            var stun = gameObj.AddComponent<StunState>();

            stun.secondsTimeout = time;

            Add(stun);

            return stun;

        }

        public void Clear()
            => items.Clear();

        public IEnumerator<StateData> GetEnumerator()
            => items.GetEnumerator();

        public void Remove(StateData data) {
            items.Remove(data);
            data.DeApply();
            Destroy(data.gameObject);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => items.GetEnumerator();
    }
}
