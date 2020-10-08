using System.Linq;
using Assets.Scripts.Character;
using Assets.Scripts.Observable;
using Assets.Scripts.Spells;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.SpellsList {
    public class SpellListRow : MonoBehaviour {

        [SerializeField]
        private Text _titleText;

        [SerializeField]
        private Text _descriptionText;

        private SubsribeTextResult[] subsribeTextResults;

        private Spell _spell;

        public void SetSpell(Spell spell, CharacterData owner) {

            _spell = spell;

            subsribeTextResults = new[] {
                _titleText.Subscribe(() => spell.GetTitle(owner), OrderVal.UIUpdate, spell.GetObservablesForModify(owner)),
                _descriptionText.Subscribe(() => spell.GetDescription(owner), OrderVal.UIUpdate, spell.GetObservablesForModify(owner))
            };

        }

        private void OnDestroy() {

            if (subsribeTextResults?.Any() ?? false) {

                foreach (var str in subsribeTextResults)
                    str.Unsubscribe();

            }

        }

    }

}
