using System;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StarsData {

    public interface IStatField { 
        Stat stat { get; }
    }

    [Serializable]
    public class StatField : IStatField {

        public Stat statType;

        [SerializeReference]
        public ObservableVal observableVal;

        public Stat stat => statType;

    }

    [Serializable]
    public class StatField<T> : ObservableVal<T>, IStatField {

        public StatField(Stat stat, T val) : base(val) {
            statType = stat;
        }

        [HideInInspector]
        public Stat statType;

        public Stat stat => statType;

        public void ModifyBy(IModifiedObservable obsrVal, bool alterVal = false) 
            => (obsrVal as IModifiedObservable<T>).ModifyTarget(this, alterVal);

    }

}
