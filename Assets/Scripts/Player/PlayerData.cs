using Assets.Scripts.Items;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Player {

    public class PlayerData : MonoBehaviour, IHaveItemsContainer {

        public ObservableVal<float> money = new ObservableVal<float>(1000);

        public ItemsContainer itemsContainer;

        public void Init() {
            itemsContainer.SetOwner(this);
        }

        public void OnAddItem(ItemData item) {
        }

        public void OnRemoveItem(ItemData item) {
        }

    }

}
