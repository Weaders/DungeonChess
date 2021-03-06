﻿using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StatsData {

    [CreateAssetMenu(menuName = "Common/Data")]
    public class StatsInfoData : ScriptableObject {

        public StatInfo[] stats;

        public StatInfo this[Stat t] => stats.First(s => s.statType == t);

    }

    [Serializable]
    public class StatInfo {

        public Stat statType;
        public Sprite sprite;
        public Stat maxStatType;
        public bool isHaveMaxStatType;

    }

}
