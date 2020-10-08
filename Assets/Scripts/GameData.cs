using System.Linq;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts {

    [CreateAssetMenu(menuName = "GameData")]
    public class GameData : ScriptableObject {

        public ItemsPool[] itemsPools;

        public DropChance[] GetDropChances(int lvl) 
            => itemsPools.First(i => i.level == lvl).itemsWithChanches;

    }
}
