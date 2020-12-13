using Assets.Scripts.Character;
using UnityEngine;

namespace Assets.Scripts.UI {

    public class StatsGrid : MonoBehaviour {

        [SerializeField]
        private StatsGridCell gridCellPrefab;

        [SerializeField]
        private CellWhatDisplay[] whatDisplay;

        public void SetCharacter(CharacterData characterData) {

            foreach (Transform tr in transform) {
                Destroy(tr.gameObject);
            }

            foreach (var d in whatDisplay) {

                var cell = Instantiate(gridCellPrefab, transform);
                cell.SetCharacter(characterData, d);

            }

        }

    }
}
