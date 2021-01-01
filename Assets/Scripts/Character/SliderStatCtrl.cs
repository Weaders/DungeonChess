using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Character {

    public class SliderStatCtrl : MonoBehaviour {

        [SerializeField]
        private Slider slider;

        public void SetValForObserve(ObservableVal<int> current, ObservableVal<int> max) {

            current.onPostChange.AddSubscription(OrderVal.UIUpdate, () => {
                slider.value = (float)current.val / max.val;
            });

            max.onPostChange.AddSubscription(OrderVal.UIUpdate, () => {
                slider.value = (float)current.val  / max.val;
            });

            slider.value = current.val / max.val;

        }

    }

}
