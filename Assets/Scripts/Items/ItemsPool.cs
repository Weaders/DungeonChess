using Assets.Scripts.EnemyData;
using UnityEngine;
using static Assets.Scripts.EnemyData.DungeonData;

namespace Assets.Scripts.Items {

    [CreateAssetMenu(menuName = "ItemsPool")]
    public class ItemsPool : ScriptableObject {

        public DropChance[] itemsWithChanches;

        public RangeRooms rooms;

    }
}
