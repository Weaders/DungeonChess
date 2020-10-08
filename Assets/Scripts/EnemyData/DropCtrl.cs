using Assets.Scripts.Character;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.EnemyData {

    public class DropCtrl : MonoBehaviour {

        //private ItemsContainer targetItemsContainer;

        public ItemsContainer targetItemsContainer { get; set; }

        public void AddDrop(CharacterCtrl ctrl, DropChance[] sortedDropChances) {

            //ctrl.characterData.stats.isDie.onPostChange.AddSubscription(Observable.OrderVal.Internal, () => {

            //    var random = Random.value;

            //    foreach (var drop in sortedDropChances) {

            //        if (1f - drop.chance < random) {

            //            targetItemsContainer.AddPrefab(drop.items[Random.Range(0, drop.items.Length - 1)]);
            //            break;

            //        }

            //    }

            //});

        }

    }
}
