using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Effects;
using Assets.Scripts.EnemyData;
using Assets.Scripts.Items;
using Assets.Scripts.State;
using Assets.Scripts.StatsData;
using UnityEngine;

namespace Assets.Scripts {

    [CreateAssetMenu(menuName = "GameData")]
    public class GameData : ScriptableObject {

        public ItemsPool[] itemsPools;

        public StatsInfoData statsInfoData;

        public StaticStatesData staticStatesData;

        public EffectObj onGetGoodEffect;

        public EffectObj healingEffect;

        public Sprite playerManaIcon;

        public Sprite critIcon;

        public int victoryLevel;

        public IEnumerable<DropChance> GetDropChances(int lvl)
            => itemsPools.Where(i => i.rooms.IsInRange(lvl)).SelectMany(i => i.itemsWithChanches);

        public ItemData[] GetRandomItemsPrefabs(int count) {

            var drops = GetDropChances(GameMng.current.level);

            ItemData[] result = new ItemData[count];

            var items = drops
                    .SelectMany(d => d.items.Select(i => new { d.chance, itemData = i }))
                    .OrderByDescending(i => i.chance)
                    .Select(i => i.itemData);

            for (var o = 0; o < count; o++) {

                var selectedItem = items.RandomElement();

                items = items.Where(i => i != selectedItem);

                result[o] = selectedItem;

            }

            return result;

        }

    }

}
