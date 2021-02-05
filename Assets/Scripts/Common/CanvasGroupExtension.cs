using UnityEngine;

namespace Assets.Scripts.Common {
    public static class CanvasGroupExtension {

        public static void Show(this CanvasGroup c) {

            c.alpha = 1;
            c.interactable = true;
            c.blocksRaycasts = true;

        }

        public static void Hide(this CanvasGroup c) {

            c.alpha = 0;
            c.interactable = false;
            c.blocksRaycasts = false;

        }

        public static bool IsShowed(this CanvasGroup c)
            => c.alpha == 1;
    }
}
