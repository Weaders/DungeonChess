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

    }

}
