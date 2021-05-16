using System.Collections.Generic;
using Assets.Scripts.Character;
using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CharacterInfoPopup {

    [RequireComponent(typeof(CanvasGroup))]
    public class CharacterInfoPopupUI : MonoBehaviour {

        [SerializeField]
        private CharacterSpellDescribeRow characterSpellDescribeRowPrefab;

        [SerializeField]
        private CharacterBuffDescribeRow characterBuffDescribeRowPrefab;

        [SerializeField]
        private VerticalLayoutGroup spellsTransform;

        [SerializeField]
        private VerticalLayoutGroup buffsTransform;

        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Reset() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetData(CharacterCtrl characterCtrl) {

            foreach (Transform tr in spellsTransform.transform)
                Destroy(tr.gameObject);

            foreach (Transform tr in buffsTransform.transform)
                Destroy(tr.gameObject);

            var spells = new List<CharacterSpellDescribeRow>(characterCtrl.characterData.spellsContainer.Count);

            foreach (var spell in characterCtrl.characterData.spellsContainer) {

                var row = Instantiate(characterSpellDescribeRowPrefab, spellsTransform.transform);

                row.SetData(spell, characterCtrl);

                spells.Add(row);

            }

            var buffs = new List<CharacterBuffDescribeRow>(characterCtrl.characterData.buffsContainer.Count);

            foreach (var buff in characterCtrl.characterData.buffsContainer) {

                var row = Instantiate(characterBuffDescribeRowPrefab, buffsTransform.transform);

                row.SetData(buff);

                buffs.Add(row);

            }

            Canvas.ForceUpdateCanvases();

            foreach (var row in buffs) 
                row.RecalcHeight();
            

            foreach (var row in spells)
                row.RecalcHeight();

            // This for what, i love unity!
            buffsTransform.enabled = false;
            buffsTransform.enabled = true;

            spellsTransform.enabled = false;
            spellsTransform.enabled = true;

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
