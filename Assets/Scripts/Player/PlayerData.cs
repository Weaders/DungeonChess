using Assets.Scripts.Items;
using Assets.Scripts.Observable;
using UnityEngine;

namespace Assets.Scripts.Player {

    public class PlayerData : MonoBehaviour, IHaveItemsContainer {

        public ObservableVal<int> money = new ObservableVal<int>(1000);

        public ObservableVal<int> charactersCount = new ObservableVal<int>(0);

        public ObservableVal<int> maxCharacterCount = new ObservableVal<int>(4);

        public ItemsContainer itemsContainer;

        public void Init() {
            itemsContainer.SetOwner(this);
        }

    }

}
