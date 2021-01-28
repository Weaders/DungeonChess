using System;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/DropChance")]
    public class DropChance : ScriptableObject, IComparable<DropChance> {

        public float chance;
        public ItemData[] items;

        public int CompareTo(DropChance other) => chance.CompareTo(other.chance);

    }
}
