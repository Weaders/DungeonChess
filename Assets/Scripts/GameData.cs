using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Items;
using Assets.Scripts.StarsData;
using UnityEngine;

namespace Assets.Scripts {

    [CreateAssetMenu(menuName = "GameData")]
    public class GameData : ScriptableObject {

        public ItemsPool[] itemsPools;

        public StatsInfoData statsInfoData;

        public IEnumerable<DropChance> GetDropChances(int lvl)
            => itemsPools.Where(i => i.rooms.IsInRange(lvl)).SelectMany(i => i.itemsWithChanches);

        public ItemData[] GetRandomItemsPrefabs(int count) {

            var drops = GetDropChances(GameMng.current.level);

            ItemData[] result = new ItemData[count];

            for (var o = 0; o < count; o++) {

                var items = drops
                .SelectMany(d => d.items.Select(i => new { d.chance, itemData = i }))
                .OrderByDescending(i => i.chance);

                var randomVal = UnityEngine.Random.value;

                ItemData selectedItem = null;

                foreach (var item in items) {

                    if ((selectedItem == null || item.chance >= randomVal) && !result.Contains(item.itemData))
                        selectedItem = item.itemData;

                }

                result[o] = selectedItem;

            }

            return result;

        }

    }

}
