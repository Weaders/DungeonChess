using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI {
    public class ToggleCharacterPanel : MonoBehaviour {

        public CanvasGroup[] panels;

        public void ShowPanel(int index) {

            for (var i = 0; i < panels.Length; i++) {

                if (i == index) {
                    panels[i].Show();
                } else {
                    panels[i].Hide();
                }

            }

        }

    }

}
