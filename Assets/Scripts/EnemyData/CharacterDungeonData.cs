using System.Linq;
using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/CharacterCtrlData")]
    public class CharacterDungeonData : ScriptableObject {

        /// <summary>
        /// Prefab of character ctrl
        /// </summary>
        public CharacterCtrl characterCtrl;

        public StatGroup[] data;

        public StatGroup GetStatGroup(int level, bool fallbackGetMax = false) {

            StatGroup maxGroup = null;

            foreach (var statGroup in data) {

                if (maxGroup == null)
                    maxGroup = statGroup;
                else if (maxGroup.levelOfDifficult < statGroup.levelOfDifficult)
                    maxGroup = statGroup;

                if (statGroup.levelOfDifficult == level)
                    return statGroup;

            }

            if (fallbackGetMax)
                return maxGroup;

            return data.First(d => d.levelOfDifficult == -1);

        }

    }

}
