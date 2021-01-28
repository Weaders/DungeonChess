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

        public ObservableVal observableVal
            => statType.GetObservableVal(this);

        public int intVal;
        public float floatVal;
        public bool boolVal;

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

        /// <summary>
        /// Called by reflection <see cref="Stats.Mofify(StatField, ModifyType)"/>
        /// </summary>
        /// <param name="obsrVal"></param>
        /// <param name="modifyType"></param>
        public void ModifyBy(IModifiedObservable obsrVal, ModifyType modifyType = ModifyType.Plus)
            => (obsrVal as IModifiedObservable<T>).ModifyTarget(this, modifyType);

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
