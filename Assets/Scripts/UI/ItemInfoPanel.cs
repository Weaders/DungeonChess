using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
    public class ItemInfoPanel : MonoBehaviour {

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text description;

        public void SetItemData(ItemData itemData) {

            title.text = itemData.title;
            description.text = itemData.description;

        }

    }

}
