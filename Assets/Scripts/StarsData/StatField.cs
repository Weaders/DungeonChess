using System;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.StarsData {

    [Serializable]
    public class StatField {

        public Stat statType;

        [SerializeReference]
        public ObservableVal observableVal;

    }

    [Serializable]
    public class StatField<T> {

        public StatField(Stat stat, T val) {

            statType = stat;
            observableVal = new ObservableVal<T>(val);

        }

        public Stat statType;

        public  ObservableVal<T> observableVal;

    }

}
