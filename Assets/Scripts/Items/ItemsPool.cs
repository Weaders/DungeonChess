using Assets.Scripts.EnemyData;
using UnityEngine;

namespace Assets.Scripts.Items {


    [CreateAssetMenu(menuName = "ItemsPool")]
    public class ItemsPool : ScriptableObject {

        public DropChance[] itemsWithChanches;

        public int level;

    }
}
