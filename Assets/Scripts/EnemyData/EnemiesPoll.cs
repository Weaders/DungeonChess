using System;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Enemies/Poll")]
    public class EnemiesPoll : ScriptableObject {

        public EnemyData[] enemies;

    }


    [Serializable]
    public class EnemyData {

        public CharacterCtrl characterCtrl;
        public DropChance[] dropChances;

        private bool isSorted = false;

        public DropChance[] sortedDropChanges {
            get {

                if (!isSorted) {
                    isSorted = true;
                    Array.Sort(dropChances);
                }

                return dropChances;

            }
        }

    }

}
