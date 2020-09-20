using UnityEngine;

namespace Assets.Scripts.EnemyData {

    [CreateAssetMenu(menuName = "Dungeon/Data")]
    public class DungeonData : ScriptableObject {

        public EnemiesPoll enemiesPoll;

        public int moneyVictory;

    }

}
