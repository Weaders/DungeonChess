using Assets.Scripts.Spells;
using UnityEngine;

namespace Assets.Scripts.UI.SpellsList {

    public class CharacterSpellsList : MonoBehaviour {

        [SerializeField]
        private SpellListRow rowPrefab;

        public void SetSpellsContainer(SpellContainer spellsContainer) {

            foreach (Transform tr in transform) {
                Destroy(tr.gameObject);
            }

            foreach (var spell in spellsContainer) {

                var row = Instantiate(rowPrefab.gameObject, transform);
                row.GetComponent<SpellListRow>().SetSpell(spell, spellsContainer.owner);

            }


        }

    }
}
