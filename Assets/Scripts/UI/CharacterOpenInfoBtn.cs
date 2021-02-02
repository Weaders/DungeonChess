using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {

    public class CharacterOpenInfoBtn : MonoBehaviour {

        [SerializeField]
        private Button button;

        private void Reset() {
            button = GetComponent<Button>();
        }

        private void Awake() {
            button.interactable = false;
        }

        public void Init() {

            GameMng.current.gameInputCtrl.onChangeSelectCharacter.AddListener((from, to) => {
                button.interactable = to != null;
            });

        }

    }
}
