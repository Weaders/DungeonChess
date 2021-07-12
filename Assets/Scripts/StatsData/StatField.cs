using System;
using System.Collections.Generic;
using Assets.Scripts.ActionsData;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StatsData {

    public enum ChangeStatType { 
        None, Hp, PercentHp, Ad, As, CritChange
    }

    public interface IStatField {
    }

    /// <summary>
    /// Used modify stat, by modift or create <see cref="StatChange"/>
    /// </summary>
    [Serializable]
    public class StatField : IStatField {

        public ChangeStatType changeStatType;

        public ObservableVal observableVal
            => changeStatType.GetObservableVal(this);

        public int intVal;
        public float floatVal;
        public bool boolVal;

    }

    public abstract class StatChange {

        public StatChange(ISource source)
            => sourceChange = source;

        public ISource sourceChange { get; private set; }
    }

    public class StatChange<T> : StatChange {

        public delegate IModifiedObservable<T> StatCalc(StatField<T> s);

        public StatChange(StatCalc func, ISource source) : base(source) {
            onCalc = func;
        }

        public StatChange(IModifiedObservable<T> modifiedObservable, ISource source) : base(source) {
            onCalc = _ => modifiedObservable;
        }

        public StatCalc onCalc { get; private set; }

        public IModifiedObservable<T> amountOfChange { get; private set; }

        public void Calc(StatField<T> stat) {
            amountOfChange = onCalc.Invoke(stat);
        }

    }


    /// <summary>
    /// Generic stat field, used with <see cref="Stat"/> type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class StatField<T> : ObservableVal<T>, IStatField {

        public StatField(Stat stat, T val) : base(val) {
            statType = stat;
        }

        [HideInInspector]
        public Stat statType;

        public Stat stat => statType;

        private List<StatChange<T>> statChanges = new List<StatChange<T>>();

        /// <summary>
        /// Called by reflection <see cref="Stats.Modify(StatField, ModifyType)"/>
        /// </summary>
        /// <param name="obsrVal"></param>
        /// <param name="modifyType"></param>
        public void ModifyBy(IModifiedObservable obsrVal, ModifyType modifyType = ModifyType.Plus)
            => (obsrVal as IModifiedObservable<T>).ModifyTarget(this, modifyType);

        /// <summary>
        /// Add change for stat
        /// </summary>
        /// <param name="change"></param>
        public StatChange<T> AddChange(StatChange<T> change) {

            statChanges.Add(change);

            change.Calc(this);

            change.amountOfChange.ModifyTarget(this, ModifyType.Plus);

            return change;

        }


        /// <summary>
        /// Remove change
        /// </summary>
        /// <param name="change"></param>
        public void RemoveChange(StatChange<T> change) {

            statChanges.Remove(change);
            change.amountOfChange.ModifyTarget(this, ModifyType.Minus);

        }

        /// <summary>
        /// Recalc changes return amount of 
        /// </summary>
        public void Recalc() {

            foreach (var stat in statChanges) {

                var oldVal = stat.amountOfChange;

                stat.Calc(this);

                var newVal = stat.amountOfChange;

                if (oldVal != newVal) {

                    ModifyBy(oldVal, ModifyType.Minus);
                    ModifyBy(newVal, ModifyType.Plus);

                }

            }

        }

    }

    /// <summary>
    /// Decorator there for display percent
    /// </summary>
    [Serializable]
    public class PercentStatField : StatField<float> {

        public PercentStatField(StatField<float> oldStat) : base(oldStat.stat, oldStat.val) {
        }

        public override string ToString() => (val * 100f).ToString();

    }

}
