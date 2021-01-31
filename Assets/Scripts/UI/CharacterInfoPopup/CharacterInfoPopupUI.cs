using Assets.Scripts.Character;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.UI.CharacterInfoPopup {

    [RequireComponent(typeof(CanvasGroup))]
    public class CharacterInfoPopupUI : MonoBehaviour {

        [SerializeField]
        private CharacterSpellDescribeRow characterSpellDescribeRowPrefab;

        [SerializeField]
        private CharacterBuffDescribeRow characterBuffDescribeRowPrefab;

        [SerializeField]
        private Transform spellsTransform;

        [SerializeField]
        private Transform buffsTransform;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetData(CharacterCtrl characterCtrl) {

            foreach (Transform tr in spellsTransform)
                Destroy(tr.gameObject);

            foreach (Transform tr in buffsTransform)
                Destroy(tr.gameObject);

            foreach (var spell in characterCtrl.characterData.spellsContainer) {

                var row = Instantiate(characterSpellDescribeRowPrefab, spellsTransform);

                row.SetData(spell, characterCtrl);

            }

            foreach (var buff in characterCtrl.characterData.buffsContainer) {

                var row = Instantiate(characterBuffDescribeRowPrefab, buffsTransform);

                row.SetData(buff);

            }

        }

        public void Show() {

            var selectedCharacter = GameMng.current.gameInputCtrl.selectedCharacterCtrl;

            SetData(selectedCharacter);

            canvasGroup.Show();

        }

        public void Hide()
            => canvasGroup.Hide();

    }

}
