using System;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StatsData {

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

    /// <summary>
    /// Decorator there for display percent
    /// </summary>
    [Serializable]
    public class PercentStatField : StatField<float> {

        public PercentStatField(StatField<float> oldStat) : base(oldStat.stat, oldStat.val) {
        }

        public override string ToString() => (val* 100f).ToString();

    }

}
