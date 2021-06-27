using UnityEngine;

namespace Assets.Scripts.Character {

    public class MessageContainer : MonoBehaviour {

        private RectTransform[] rects;

        private int currentRectIndex = 0;

        public RectTransform GetRect() {

            if (rects == null || rects.Length == 0) {

                rects = new RectTransform[transform.childCount];

                for (var i = 0; i < rects.Length; i++)
                    rects[i] = transform.GetChild(i).GetComponent<RectTransform>();

            }

            var rect = rects[currentRectIndex];

            if (++currentRectIndex == rects.Length) {
                currentRectIndex = 0;
            }

            return rect;

        }

    }

}
