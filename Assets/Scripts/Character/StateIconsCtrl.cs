using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Character {
    public class StateIconsCtrl : MonoBehaviour {

        [SerializeField]
        private Image iconPrefab;

        private CharacterData _characterData;

        public void SetCharacterData(CharacterData characterData) {

            _characterData = characterData;

            _characterData.stateContainer.onAdd.AddSubscription(Observable.OrderVal.UIUpdate, RefreshIcons);
            _characterData.stateContainer.onRemove.AddSubscription(Observable.OrderVal.UIUpdate, RefreshIcons);

            RefreshIcons();

        }

        public void RefreshIcons() {

            foreach (Transform tr in transform)
                Destroy(tr.gameObject);

            foreach (var buff in _characterData.stateContainer) {

                var obj = Instantiate(iconPrefab, transform);
                obj.sprite = buff.icon;

            }

        }

    }

}
