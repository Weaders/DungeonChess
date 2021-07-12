using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Observable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
    public class ProgressBarCtrl : MonoBehaviour {

        [SerializeField]
        private Image img;

        [SerializeField]
        private Slider slider;

        //public bool isSmoothly = true;

        private Dictionary<Image, Coroutine> coroutinesForUpdate = new Dictionary<Image, Coroutine>();

        public void SetValForObserve(ObservableVal<int> current, ObservableVal<int> max) {

            current.onPostChange.AddSubscription(OrderVal.UIUpdate, () => {

                if (slider != null)
                    slider.value = (float)current.val / max.val;

                if (img != null && img.isActiveAndEnabled) {

                    if (coroutinesForUpdate.TryGetValue(img, out var coro)) {

                        if (coro != null)
                            StopCoroutine(coro);

                        coroutinesForUpdate.Remove(img);

                    }

                    coroutinesForUpdate.Add(img, StartCoroutine(SmoothUpdate(img, (float)current.val / max.val)));

                }


            });

            max.onPostChange.AddSubscription(OrderVal.UIUpdate, () => {

                if (slider != null)
                    slider.value = (float)current.val / max.val;

                if (img != null) {

                    if (coroutinesForUpdate.TryGetValue(img, out var coro)) {
                        StopCoroutine(coro);
                        coroutinesForUpdate.Remove(img);
                    }

                    img.fillAmount = (float)current.val / max.val;

                }

            });

            if (slider != null)
                slider.value = (float)current.val / max.val;

            if (img != null)
                img.fillAmount = (float)current.val / max.val;

        }

        public IEnumerator SmoothUpdate(Image img, float newVal, float step = 2f) {

            var oldVal = img.fillAmount;

            var t = 0f;

            while (img.fillAmount != newVal) {

                t += step * Time.deltaTime;

                if (t >= 1f)
                    t = 1f;

                img.fillAmount = Mathf.Lerp(oldVal, newVal, t);

                yield return null;
            }

        }

    }
}
